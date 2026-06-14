using Microsoft.EntityFrameworkCore;
using BitcoinMarketLoader.Domain.Market;

namespace BitcoinMarketLoader.Infrastructure.Databases;

public class MarketRepository(MainDbContext db) : IMarketRepository
{
    public Task<int> GetMarketTicksCount() => db.MarketTicks.CountAsync();

    public async Task<ICollection<MarketTick>> GetAllMarketTicks(bool includeDetails = false)
    {
        var query = GetMarketTickQuery(includeDetails);
        var ticks = await query
            .ToListAsync()
            .ConfigureAwait(false);
        return ticks;
    }

    public async Task<ICollection<MarketTick>> GetMarketTicks(int skip, int take, bool includeDetails = false)
    {
        var query = GetMarketTickQuery(includeDetails);
        var ticks = await query
            .OrderByDescending(t => t.Ccseq)
            .Skip(skip)
            .Take(take)
            .ToListAsync()
            .ConfigureAwait(false);
        return ticks;
    }

    public async Task<MarketTick?> GetMarketTick(long ccseq, bool includeDetails = false)
    {
        var query = GetMarketTickQuery(includeDetails);
        var tick = await query
            .FirstOrDefaultAsync(t => t.Ccseq == ccseq)
            .ConfigureAwait(false);
        return tick;
    }

    public async Task<bool> AddMarketTick(MarketTick marketTick)
    {
        ArgumentNullException.ThrowIfNull(marketTick);

        if (await db.MarketTicks
                .AnyAsync(tick => tick.Ccseq == marketTick.Ccseq)
                .ConfigureAwait(false))
        {
            return false;
        }

        var tradeStatistics = marketTick.TradeStatistics?.ToList() ?? [];
        await using var transaction = await db.Database
            .BeginTransactionAsync()
            .ConfigureAwait(false);

        try
        {
            marketTick.BaseCurrency = await UpsertCurrency(marketTick.BaseCurrency)
                .ConfigureAwait(false);
            marketTick.QuoteCurrency = await UpsertCurrency(marketTick.QuoteCurrency)
                .ConfigureAwait(false);

            // Statistics are persisted after their source tick to satisfy the FK constraint.
            marketTick.TradeStatistics = null;

            db.MarketTicks.Add(marketTick);
            await db.SaveChangesAsync().ConfigureAwait(false);

            foreach (var statistics in tradeStatistics)
            {
                statistics.SourceTickCcseq = marketTick.Ccseq;
                statistics.SourceTick = marketTick;
                await UpsertTradeStatistics(statistics).ConfigureAwait(false);
            }

            await db.SaveChangesAsync().ConfigureAwait(false);
            await transaction.CommitAsync().ConfigureAwait(false);
            return true;
        }
        catch
        {
            await transaction.RollbackAsync().ConfigureAwait(false);
            throw;
        }
        finally
        {
            marketTick.TradeStatistics = tradeStatistics;
        }
    }
    
    public async Task<bool> UpdateTickNote(long ccseq, string? note)
    {
        var tick = await db.MarketTicks.FindAsync(ccseq).ConfigureAwait(false);
        if (tick is not null)
        {
            tick.Note = note;
            var updated = await db.SaveChangesAsync();
            return updated > 0;
        }
        return false;
    }
    
    private IQueryable<MarketTick> GetMarketTickQuery(bool includeDetails)
    {
        IQueryable<MarketTick> query = db.MarketTicks
            .AsNoTracking()
            .Include(tick => tick.BaseCurrency)
            .Include(tick => tick.QuoteCurrency);

        return includeDetails ? IncludeDetails(query) : query;
    }
    
    private IQueryable<MarketTick> IncludeDetails(IQueryable<MarketTick> query) => query
        .Include(m => m.TradeStatistics)
        .Include(m => m.LastTrade)
        .Include(m => m.LastProcessedTrade)
        .Include(m => m.BestAsk)
        .Include(m => m.BestBid);

    private async Task<MarketCurrency?> UpsertCurrency(MarketCurrency? currency)
    {
        if (currency is null)
        {
            return null;
        }

        var trackedCurrency = db.MarketCurrencies.Local
            .FirstOrDefault(existing => existing.Id == currency.Id);
        var existingCurrency = trackedCurrency ?? await db.MarketCurrencies
            .FindAsync(currency.Id)
            .ConfigureAwait(false);

        if (existingCurrency is null)
        {
            db.MarketCurrencies.Add(currency);
            await db.SaveChangesAsync().ConfigureAwait(false);
            return currency;
        }

        if (existingCurrency.Name != currency.Name)
        {
            existingCurrency.Name = currency.Name;
            await db.SaveChangesAsync().ConfigureAwait(false);
        }

        return existingCurrency;
    }

    private async Task UpsertTradeStatistics(TradeStatistics statistics)
    {
        var existingStatistics = await db.TradeStatistics.FindAsync(
                statistics.SourceTickCcseq,
                statistics.Range)
            .ConfigureAwait(false);

        if (existingStatistics is null)
        {
            db.TradeStatistics.Add(statistics);
            return;
        }

        db.Entry(existingStatistics).CurrentValues.SetValues(statistics);
        existingStatistics.HighPriceTimestamp = statistics.HighPriceTimestamp;
        existingStatistics.LowPriceTimestamp = statistics.LowPriceTimestamp;
        existingStatistics.FirstTradeTimestamp = statistics.FirstTradeTimestamp;
    }
}
