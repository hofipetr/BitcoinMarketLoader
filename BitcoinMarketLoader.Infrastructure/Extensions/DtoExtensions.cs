using System.Text.Json;
using BitcoinMarketLoader.Domain.Enums;
using BitcoinMarketLoader.Domain.Market;
using BitcoinMarketLoader.Infrastructure.Dtos.MarketTickDtos;

namespace BitcoinMarketLoader.Infrastructure.Extensions;

/// <summary>
/// Helper mapper that converts MarketTick DTOs to domain <see cref="MarketTick"/> instances.
/// Implemented as explicit mapping (no source-generator) to handle the wide variety of field
/// name prefixes present in the DTO. The mapping is deterministic — the first instrument in
/// the DTO.Data dictionary is used as the source of the tick.
/// </summary>
public static class MarketTickDtoExtensions
{
    extension(MarketTickDto dto)
    {
        public MarketTick ToDomainModel()
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.Data == null || dto.Data.Count == 0) throw new ArgumentException("MarketTickDto contains no instrument data", nameof(dto));

            var instrument = dto.Data.Values.First();
            if (!instrument.Ccseq.HasValue) throw new ArgumentException("MarketTickDto instrument doesn't contain mandatory property CCSEQ", nameof(dto));
            var tickKey = instrument.Ccseq.Value;
            
            var ranges = Enum.GetValues<TimeWindowRanges>();
            var tradeStatistics = ranges.Select(r => instrument.TradeStatistics(r, tickKey)).ToList();
            
            var tick = new MarketTick
            {
                Ccseq = tickKey,
                Market = instrument.Market,
                Instrument = instrument.Instrument,
                BaseCurrency = instrument.BaseCurrency(),
                QuoteCurrency = instrument.QuoteCurrency(),
                
                Price = instrument.Price,
                PriceFlag = instrument.PriceFlag,
                PriceUpdateTimestamp = BuildTimestamp(instrument.PriceLastUpdateTs, instrument.PriceLastUpdateTsNs),
                
                LastTrade = instrument.LastTrade(),
                LastProcessedTrade = instrument.LastProcessedTrade(),
                BestBid = instrument.BestBid(),
                BestAsk = instrument.BestAsk(),
                TradeStatistics = tradeStatistics,
            };

            return tick;
        }
    }

    extension(InstrumentDataDto src)
    {
        public TradeDetails? LastTrade()
        {
            if (src.LastTradeCcseq == null) return null;

            return new TradeDetails
            {
                Ccseq = src.LastTradeCcseq.Value,
                TradeId = src.LastTradeId,
                Quantity = src.LastTradeQuantity,
                QuoteQuantity = src.LastTradeQuoteQuantity,
                Timestamp = BuildTimestamp(src.LastTradeReceivedTs, src.LastTradeReceivedTsNs),
                ReceivedTimestamp = BuildTimestamp(src.LastTradeReceivedTs, src.LastTradeReceivedTsNs),
                Side = ParseTradeSide(src.LastTradeSide)
            };
        }

        public TradeDetails? LastProcessedTrade()
        {
            if (src.LastProcessedTradeCcseq == null) return null;

            return new TradeDetails
            {
                Ccseq = src.LastProcessedTradeCcseq.Value,
                Quantity = src.LastProcessedTradeQuantity,
                QuoteQuantity = src.LastProcessedTradeQuoteQuantity,
                Price = src.LastProcessedTradePrice,
                Timestamp = BuildTimestamp(src.LastProcessedTradeTs, src.LastProcessedTradeTsNs),
                ReceivedTimestamp = BuildTimestamp(src.LastProcessedTradeReceivedTs, src.LastProcessedTradeReceivedTsNs),
                Side = ParseTradeSide(src.LastProcessedTradeSide),
            };
        }

        public MarketCurrency? BaseCurrency()
        {
            if (src.BaseId is null) return null;
            return new MarketCurrency
            {
                Id = src.BaseId.Value,
                Name = src.Base
            };
        }

        public MarketCurrency? QuoteCurrency()
        {
            if (src.QuoteId is null) return null;
            return new MarketCurrency
            {
                Id = src.QuoteId.Value,
                Name = src.Quote
            };
        }

        public Order BestBid()
        {
            return new Order
            {
                Price = src.BestBid,
                Quantity = src.BestBidQuantity,
                QuoteQuantity = src.BestBidQuoteQuantity,
                LastUpdateTimestamp = BuildTimestamp(src.BestBidLastUpdateTs, src.BestBidLastUpdateTsNs),
                PositionUpdateTimestamp = BuildTimestamp(src.BestBidPositionInBookUpdateTs, src.BestBidPositionInBookUpdateTsNs)
            };
        }

        public Order BestAsk()
        {
            return new Order
            {
                Price = src.BestAsk,
                Quantity = src.BestAskQuantity,
                QuoteQuantity = src.BestAskQuoteQuantity,
                LastUpdateTimestamp = BuildTimestamp(src.BestAskLastUpdateTs, src.BestAskLastUpdateTsNs),
                PositionUpdateTimestamp = BuildTimestamp(src.BestAskPositionInBookUpdateTs, src.BestAskPositionInBookUpdateTsNs)
            };
        }

        public TradeStatistics TradeStatistics(TimeWindowRanges range, long tickKey)
        {
            var prefix = range switch
            {
                TimeWindowRanges.CurrentHour => "CURRENT_HOUR",
                TimeWindowRanges.CurrentDay => "CURRENT_DAY",
                TimeWindowRanges.CurrentWeek => "CURRENT_WEEK",
                TimeWindowRanges.CurrentMonth => "CURRENT_MONTH",
                TimeWindowRanges.CurrentYear => "CURRENT_YEAR",
                TimeWindowRanges.Last24Hours => "MOVING_24_HOUR",
                TimeWindowRanges.Last7Days => "MOVING_7_DAY",
                TimeWindowRanges.Last30Days => "MOVING_30_DAY",
                TimeWindowRanges.Last90Days => "MOVING_90_DAY",
                TimeWindowRanges.Last180Days => "MOVING_180_DAY",
                TimeWindowRanges.Last365Days => "MOVING_365_DAY",
                TimeWindowRanges.LifeTime => "LIFETIME",
                _ => throw new ArgumentOutOfRangeException(nameof(range), range, null)
            };

            return new()
            {
                Range = range,
                SourceTickCcseq = tickKey,
                VolumeTotal = src.ExtensionData?.ValueOrDefault<decimal?>($"{prefix}_VOLUME"),
                VolumeBuy = src.ExtensionData?.ValueOrDefault<decimal?>($"{prefix}_VOLUME_BUY"),
                VolumeSell = src.ExtensionData?.ValueOrDefault<decimal?>($"{prefix}_VOLUME_SELL"),
                VolumeUnknown = src.ExtensionData?.ValueOrDefault<decimal?>($"{prefix}_VOLUME_UNKNOWN"),
                QuoteVolumeTotal = src.ExtensionData?.ValueOrDefault<decimal?>($"{prefix}_QUOTE_VOLUME"),
                QuoteVolumeBuy = src.ExtensionData?.ValueOrDefault<decimal?>($"{prefix}_QUOTE_VOLUME_BUY"),
                QuoteVolumeSell = src.ExtensionData?.ValueOrDefault<decimal?>($"{prefix}_QUOTE_VOLUME_SELL"),
                QuoteVolumeUnknown = src.ExtensionData?.ValueOrDefault<decimal?>($"{prefix}_QUOTE_VOLUME_UNKNOWN"),
                OpenPrice = src.ExtensionData?.ValueOrDefault<decimal?>($"{prefix}_OPEN"),
                HighPrice = src.ExtensionData?.ValueOrDefault<decimal?>($"{prefix}_HIGH"),
                LowPrice = src.ExtensionData?.ValueOrDefault<decimal?>($"{prefix}_LOW"),
                PriceChange = src.ExtensionData?.ValueOrDefault<decimal?>($"{prefix}_CHANGE"),
                PriceChangePercent = src.ExtensionData?.ValueOrDefault<decimal?>($"{prefix}_CHANGE_PERCENTAGE"),
                TotalTrades = src.ExtensionData?.ValueOrDefault<long?>($"{prefix}_TOTAL_TRADES"),
                TotalBuys = src.ExtensionData?.ValueOrDefault<long?>($"{prefix}_TOTAL_TRADES_BUY"),
                TotalSells = src.ExtensionData?.ValueOrDefault<long?>($"{prefix}_TOTAL_TRADES_SELL"),
                TotalUnknownTrades = src.ExtensionData?.ValueOrDefault<long?>($"{prefix}_TOTAL_TRADES_UNKNOWN"),
                HighPriceTimestamp = BuildTimestamp(src.ExtensionData?.ValueOrDefault<long?>($"{prefix}_HIGH_TS")),
                LowPriceTimestamp = BuildTimestamp(src.ExtensionData?.ValueOrDefault<long?>($"{prefix}_LOW_TS")),
                FirstTradeTimestamp = BuildTimestamp(src.ExtensionData?.ValueOrDefault<long?>($"{prefix}_FIRST_TRADE_TS")),
            };
        }
    }

    extension(IDictionary<string, object> src)
    {
        public TValue? ValueOrDefault<TValue>(string key, TValue? defaultValue = default)
        {
            if (!src.TryGetValue(key, out var value))
            {
                return defaultValue;
            }

            return value switch
            {
                TValue typedValue => typedValue,
                JsonElement jsonElement => jsonElement.Deserialize<TValue>(),
                _ => defaultValue,
            };
        }
    }
    
    /// <summary>
    /// Helper method to initialize <see cref="MarketTimestamp"/> from nullable values 
    /// </summary>
    private static MarketTimestamp? BuildTimestamp(long? seconds, long? ns = null) => seconds.HasValue 
        ? new MarketTimestamp { UnixSeconds = seconds.Value, Nanoseconds = ns ?? 0 }
        : null;

    private static TradeSides ParseTradeSide(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return TradeSides.Buy;
        return s.Equals("SELL", StringComparison.OrdinalIgnoreCase)
            ? TradeSides.Sell
            : TradeSides.Buy;
    }
}
