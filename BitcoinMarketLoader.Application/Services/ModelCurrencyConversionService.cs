using BitcoinMarketLoader.Domain.Enums;
using BitcoinMarketLoader.Domain.Extensions;
using BitcoinMarketLoader.Domain.Market;
using Microsoft.Extensions.Logging;

namespace BitcoinMarketLoader.Application.Services;

public class ModelCurrencyConversionService(
    ICzkExchangeRateService exchangeRateService,
    ILogger<ModelCurrencyConversionService> logger): IModelCurrencyConversionService
{
    private const string EurCode = "EUR";
    static readonly MarketCurrency CzkCurrency = new MarketCurrency { Name = "CZK" };
    
    public async Task<bool> ConvertEurToCzk(MarketTick model)
    {
        if (model.QuoteCurrency?.Name != EurCode)
        {
            logger.LogWarning("Cannot convert market tick {tickId} to CZK with quote currency '{currency}' - only conversion from EUR is supported", model.Ccseq, model.QuoteCurrency?.Name);
            return false;
        }
        
        // Implementation for converting EUR to CZK
        bool result = true;
        try
        {
            model.Price = await ConvertValue(model.Price, model.PriceUpdateTimestamp, model.GeneratedTimestamp).ConfigureAwait(false);
            
            await ConvertEurToCzk(model.LastTrade, model.GeneratedTimestamp).ConfigureAwait(false);
            await ConvertEurToCzk(model.LastProcessedTrade, model.GeneratedTimestamp).ConfigureAwait(false);
            
            await ConvertEurToCzk(model.BestBid, model.GeneratedTimestamp).ConfigureAwait(false);
            await ConvertEurToCzk(model.BestAsk, model.GeneratedTimestamp).ConfigureAwait(false);

            if (model.TradeStatistics?.Count > 0)
            {
                foreach (var ts in model.TradeStatistics)
                {
                    await ConvertEurToCzk(ts, model.GeneratedTimestamp, model.Price).ConfigureAwait(false);
                }
            }

            model.QuoteCurrency = CzkCurrency;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error converting market tick {tickId} from EUR to CZK", model.Ccseq);
            return false;
        }
        return result;
    }

    private async Task ConvertEurToCzk(TradeDetails? model, DateTime tickDefaultTimestamp)
    {
        if (model is null) return;
        
        model.Price = await ConvertValue(model.Price, model.Timestamp ?? model.ReceivedTimestamp, tickDefaultTimestamp).ConfigureAwait(false);
    }

    private async Task ConvertEurToCzk(Order? model, DateTime tickDefaultTimestamp)
    {
        if (model is null) return;
        
        model.Price = await ConvertValue(model.Price, model.LastUpdateTimestamp, tickDefaultTimestamp).ConfigureAwait(false);
    }

    private async Task ConvertEurToCzk(TradeStatistics? model, DateTime tickDefaultTimestamp, decimal? currentPrice)
    {
        if (model is null) return;
        var rangeStart = model.Range switch
        {
            TimeWindowRanges.LifeTime => model.FirstTradeTimestamp?.Date ?? tickDefaultTimestamp,
            _ => model.Range.RangeStart(tickDefaultTimestamp)
        };

        model.OpenPrice = await ConvertValue(model.OpenPrice, null, rangeStart).ConfigureAwait(false);
        model.HighPrice = await ConvertValue(model.HighPrice, model.HighPriceTimestamp, tickDefaultTimestamp)
            .ConfigureAwait(false);
        model.LowPrice = await ConvertValue(model.LowPrice, model.LowPriceTimestamp, tickDefaultTimestamp)
            .ConfigureAwait(false);
        
        // Total volumes - convert by rate valid for tickDefaultTimestamp - it's not possible to recalculate the total volume precisely due to changing rate during the range
        model.QuoteVolumeTotal = await ConvertValue(model.QuoteVolumeTotal, null, tickDefaultTimestamp).ConfigureAwait(false);
        model.QuoteVolumeBuy = await ConvertValue(model.QuoteVolumeBuy, null, tickDefaultTimestamp).ConfigureAwait(false);
        model.QuoteVolumeSell = await ConvertValue(model.QuoteVolumeSell, null, tickDefaultTimestamp).ConfigureAwait(false);
        model.QuoteVolumeUnknown = await ConvertValue(model.QuoteVolumeUnknown, null, tickDefaultTimestamp).ConfigureAwait(false);
        
        if (currentPrice is not null)
        {
            // recalculate change - it may be different from the EUR change due to additional changes in exchange rate
            model.PriceChange = currentPrice - model.OpenPrice;
            model.PriceChangePercent = (model.PriceChange / model.OpenPrice) * 100m;
        }
        else
        {
            // fallback - just convert price change due to unknown current price
            model.PriceChange = await ConvertValue(model.PriceChange, null, tickDefaultTimestamp).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Helper method that converts a decimal value from EUR to CZK using exchange rate valid for a given <see cref="MarketTimestamp"/> or fallback date.  
    /// </summary>
    /// <param name="value">Decimal value to convert</param>
    /// <param name="timestamp">Timestamp for which to get an exchange rate</param>
    /// <param name="fallbackTimestamp">Fallback timestamp if no specific timestamp is available</param>
    /// <returns>Converted decimal value. Null if the source value is null.</returns>
    Task<Decimal?> ConvertValue(Decimal? value, MarketTimestamp? timestamp, DateTime fallbackTimestamp) =>
        exchangeRateService.ConvertToCzk(EurCode, value, timestamp?.Date ?? fallbackTimestamp);
}

