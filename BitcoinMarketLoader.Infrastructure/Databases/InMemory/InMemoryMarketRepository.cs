using BitcoinMarketLoader.Domain.Market;

namespace BitcoinMarketLoader.Infrastructure.Databases.InMemory;

public class InMemoryMarketRepository : IMarketRepository
{
    private readonly object _syncRoot = new();
    private readonly SortedDictionary<long, MarketTick> _marketTicks = new();

    public InMemoryMarketRepository()
    {
    }

    public InMemoryMarketRepository(IEnumerable<MarketTick> marketTicks)
    {
        ArgumentNullException.ThrowIfNull(marketTicks);

        foreach (var marketTick in marketTicks)
        {
            ArgumentNullException.ThrowIfNull(marketTick);
            if (!_marketTicks.TryAdd(marketTick.Ccseq, CloneMarketTick(marketTick, true)))
            {
                throw new ArgumentException(
                    $"A market tick with CCSEQ {marketTick.Ccseq} already exists.",
                    nameof(marketTicks));
            }
        }
    }

    public Task<int> GetMarketTicksCount()
    {
        lock (_syncRoot)
        {
            return Task.FromResult(_marketTicks.Count);
        }
    }

    public Task<ICollection<MarketTick>> GetAllMarketTicks(bool includeDetails = false)
    {
        lock (_syncRoot)
        {
            ICollection<MarketTick> result = _marketTicks.Values
                .Select(tick => CloneMarketTick(tick, includeDetails))
                .ToList();

            return Task.FromResult(result);
        }
    }

    public Task<ICollection<MarketTick>> GetMarketTicks(
        int skip,
        int take,
        bool includeDetails = false)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(skip);
        ArgumentOutOfRangeException.ThrowIfNegative(take);

        lock (_syncRoot)
        {
            ICollection<MarketTick> result = _marketTicks.Values
                .Skip(skip)
                .Take(take)
                .Select(tick => CloneMarketTick(tick, includeDetails))
                .ToList();

            return Task.FromResult(result);
        }
    }

    public Task<MarketTick?> GetMarketTick(long ccseq, bool includeDetails = false)
    {
        lock (_syncRoot)
        {
            var result = _marketTicks.TryGetValue(ccseq, out var marketTick)
                ? CloneMarketTick(marketTick, includeDetails)
                : null;

            return Task.FromResult(result);
        }
    }

    public Task<bool> AddMarketTick(MarketTick marketTick)
    {
        ArgumentNullException.ThrowIfNull(marketTick);

        lock (_syncRoot)
        {
            var added = _marketTicks.TryAdd(
                marketTick.Ccseq,
                CloneMarketTick(marketTick, true));

            return Task.FromResult(added);
        }
    }

    public Task<bool> UpdateTickNote(long ccseq, string? note)
    {
        lock (_syncRoot)
        {
            if (!_marketTicks.TryGetValue(ccseq, out var marketTick))
            {
                return Task.FromResult(false);
            }

            marketTick.Note = note;
            return Task.FromResult(true);
        }
    }

    private static MarketTick CloneMarketTick(MarketTick source, bool includeDetails) =>
        new()
        {
            Ccseq = source.Ccseq,
            Market = source.Market,
            Instrument = source.Instrument,
            BaseCurrency = CloneCurrency(source.BaseCurrency),
            QuoteCurrency = CloneCurrency(source.QuoteCurrency),
            LastTrade = CloneTradeDetails(source.LastTrade),
            LastProcessedTrade = CloneTradeDetails(source.LastProcessedTrade),
            Price = source.Price,
            PriceFlag = source.PriceFlag,
            PriceUpdateTimestamp = CloneTimestamp(source.PriceUpdateTimestamp),
            BestBid = CloneOrder(source.BestBid),
            BestAsk = CloneOrder(source.BestAsk),
            Note = source.Note,
            TradeStatistics = includeDetails
                ? source.TradeStatistics?.Select(CloneTradeStatistics).ToList()
                : null,
        };

    private static MarketCurrency? CloneCurrency(MarketCurrency? source) =>
        source is null
            ? null
            : new MarketCurrency
            {
                Id = source.Id,
                Name = source.Name,
            };

    private static TradeDetails? CloneTradeDetails(TradeDetails? source) =>
        source is null
            ? null
            : new TradeDetails
            {
                Ccseq = source.Ccseq,
                TradeId = source.TradeId,
                Quantity = source.Quantity,
                QuoteQuantity = source.QuoteQuantity,
                Price = source.Price,
                Timestamp = CloneTimestamp(source.Timestamp),
                ReceivedTimestamp = CloneTimestamp(source.ReceivedTimestamp),
                Side = source.Side,
            };

    private static Order? CloneOrder(Order? source) =>
        source is null
            ? null
            : new Order
            {
                Price = source.Price,
                Quantity = source.Quantity,
                QuoteQuantity = source.QuoteQuantity,
                LastUpdateTimestamp = CloneTimestamp(source.LastUpdateTimestamp),
                PositionUpdateTimestamp = CloneTimestamp(source.PositionUpdateTimestamp),
            };

    private static TradeStatistics CloneTradeStatistics(TradeStatistics source) =>
        new()
        {
            Range = source.Range,
            SourceTickCcseq = source.SourceTickCcseq,
            VolumeTotal = source.VolumeTotal,
            VolumeBuy = source.VolumeBuy,
            VolumeSell = source.VolumeSell,
            VolumeUnknown = source.VolumeUnknown,
            QuoteVolumeTotal = source.QuoteVolumeTotal,
            QuoteVolumeBuy = source.QuoteVolumeBuy,
            QuoteVolumeSell = source.QuoteVolumeSell,
            QuoteVolumeUnknown = source.QuoteVolumeUnknown,
            OpenPrice = source.OpenPrice,
            HighPrice = source.HighPrice,
            LowPrice = source.LowPrice,
            PriceChange = source.PriceChange,
            PriceChangePercent = source.PriceChangePercent,
            HighPriceTimestamp = CloneTimestamp(source.HighPriceTimestamp),
            LowPriceTimestamp = CloneTimestamp(source.LowPriceTimestamp),
            FirstTradeTimestamp = CloneTimestamp(source.FirstTradeTimestamp),
            TotalTrades = source.TotalTrades,
            TotalBuys = source.TotalBuys,
            TotalSells = source.TotalSells,
            TotalUnknownTrades = source.TotalUnknownTrades,
        };

    private static MarketTimestamp? CloneTimestamp(MarketTimestamp? source) =>
        source is null
            ? null
            : new MarketTimestamp
            {
                UnixSeconds = source.UnixSeconds,
                Nanoseconds = source.Nanoseconds,
            };
}
