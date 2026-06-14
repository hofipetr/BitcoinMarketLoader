using System.Text.Json;
using BitcoinMarketLoader.Domain.Enums;
using BitcoinMarketLoader.Domain.Market;
using BitcoinMarketLoader.Infrastructure.Dtos.MarketTickDtos;
using BitcoinMarketLoader.Infrastructure.Extensions;

namespace BitcoinMarketLoader.UnitTests.Mapping;

public class MarketTickMappingTests
{
    private const string InstrumentCode = "BTC-EUR";
    
    [Fact]
    public void MarketTickDto_IsParsedFromJson()
    {
        // act
        var tickDto = CreateMockedTickDto();
        
        // assert
        Assert.NotNull(tickDto.Data);
        Assert.Single(tickDto.Data);
        Assert.Contains(InstrumentCode, tickDto.Data);
        var instrumentDto = tickDto.Data[InstrumentCode];

        //Basic identity/mapping
        Assert.Equal("953", instrumentDto.Type);
        Assert.Equal("coinbase", instrumentDto.Market);
        Assert.Equal("BTC-EUR", instrumentDto.Instrument);
        Assert.Equal("BTC-EUR", instrumentDto.MappedInstrument);
        Assert.Equal("BTC", instrumentDto.Base);
        Assert.Equal("EUR", instrumentDto.Quote);
        Assert.Equal(1, instrumentDto.BaseId);
        Assert.Equal(1748, instrumentDto.QuoteId);
        Assert.Equal(107656825L, instrumentDto.Ccseq);
        Assert.Equal("", instrumentDto.TransformFunction);
        Assert.Equal(54509.05m, instrumentDto.Price);
        Assert.Equal("DOWN", instrumentDto.PriceFlag);
        Assert.Equal(1781251001, instrumentDto.PriceLastUpdateTs);
        Assert.Equal(472000000, instrumentDto.PriceLastUpdateTsNs);

        // Last trades
        Assert.Equal(0.00272397m, instrumentDto.LastTradeQuantity);
        Assert.Equal(148.4810169285m, instrumentDto.LastTradeQuoteQuantity);
        Assert.Equal("107624222", instrumentDto.LastTradeId);
        Assert.Equal(107624126, instrumentDto.LastTradeCcseq);
        Assert.Equal("BUY", instrumentDto.LastTradeSide);
        Assert.Equal(1781251001, instrumentDto.LastTradeReceivedTs);
        Assert.Equal(516000000, instrumentDto.LastTradeReceivedTsNs);

        Assert.Equal(1781251001, instrumentDto.LastProcessedTradeTs);
        Assert.Equal(472000000, instrumentDto.LastProcessedTradeTsNs);
        Assert.Equal(1781251001, instrumentDto.LastProcessedTradeReceivedTs);
        Assert.Equal(516000000, instrumentDto.LastProcessedTradeReceivedTsNs);
        Assert.Equal(54509.05m, instrumentDto.LastProcessedTradePrice);
        Assert.Equal(0.00272397m, instrumentDto.LastProcessedTradeQuantity);
        Assert.Equal(148.4810169285m, instrumentDto.LastProcessedTradeQuoteQuantity);
        Assert.Equal("BUY", instrumentDto.LastProcessedTradeSide);
        Assert.Equal(107624126, instrumentDto.LastProcessedTradeCcseq);

        Assert.Equal(54507.28m, instrumentDto.BestBid);
        Assert.Equal(0.01504342m, instrumentDto.BestBidQuantity);
        Assert.Equal(819.9759060976m, instrumentDto.BestBidQuoteQuantity);
        Assert.Equal(1781251002, instrumentDto.BestBidLastUpdateTs);
        Assert.Equal(567704000, instrumentDto.BestBidLastUpdateTsNs);
        Assert.Equal(1781251002, instrumentDto.BestBidPositionInBookUpdateTs);
        Assert.Equal(567704000, instrumentDto.BestBidPositionInBookUpdateTsNs);
        Assert.Equal(54509.05m, instrumentDto.BestAsk);
        Assert.Equal(0.00025721m, instrumentDto.BestAskQuantity);
        Assert.Equal(14.0202727505m, instrumentDto.BestAskQuoteQuantity);
        Assert.Equal(1781251002, instrumentDto.BestAskLastUpdateTs);
        Assert.Equal(355097000, instrumentDto.BestAskLastUpdateTsNs);
        Assert.Equal(1781251001, instrumentDto.BestAskPositionInBookUpdateTs);
        Assert.Equal(472913000, instrumentDto.BestAskPositionInBookUpdateTsNs);

        Assert.NotNull(instrumentDto.ExtensionData);
        Assert.Equal(207, instrumentDto.ExtensionData.Count);

        AssertExtensionValue(instrumentDto.ExtensionData, "CURRENT_HOUR_VOLUME", 11.91312053m);
        AssertExtensionValue(instrumentDto.ExtensionData, "CURRENT_HOUR_QUOTE_VOLUME", 648839.426476321m);
        AssertExtensionValue(instrumentDto.ExtensionData, "CURRENT_HOUR_TOTAL_TRADES", 1269m);
        AssertExtensionValue(instrumentDto.ExtensionData, "CURRENT_HOUR_CHANGE_PERCENTAGE", 0.251709559611265m);
        AssertExtensionValue(instrumentDto.ExtensionData, "CURRENT_DAY_VOLUME", 102.3872712m);
        AssertExtensionValue(instrumentDto.ExtensionData, "CURRENT_DAY_LOW", 54259.68m);
        AssertExtensionValue(instrumentDto.ExtensionData, "CURRENT_DAY_CHANGE", -413.53m);
        AssertExtensionValue(instrumentDto.ExtensionData, "CURRENT_WEEK_VOLUME", 2177.66384045m);
        AssertExtensionValue(instrumentDto.ExtensionData, "CURRENT_WEEK_TOTAL_TRADES_BUY", 95122m);
        AssertExtensionValue(instrumentDto.ExtensionData, "CURRENT_WEEK_CHANGE_PERCENTAGE", -0.856169460148706m);
        AssertExtensionValue(instrumentDto.ExtensionData, "CURRENT_MONTH_QUOTE_VOLUME", 383028990.730228m);
        AssertExtensionValue(instrumentDto.ExtensionData, "CURRENT_MONTH_HIGH", 63514.62m);
        AssertExtensionValue(instrumentDto.ExtensionData, "CURRENT_MONTH_CHANGE", -8660.06m);
        AssertExtensionValue(instrumentDto.ExtensionData, "CURRENT_YEAR_VOLUME_SELL", 28912.6503767m);
        AssertExtensionValue(instrumentDto.ExtensionData, "CURRENT_YEAR_HIGH", 84100m);
        AssertExtensionValue(instrumentDto.ExtensionData, "CURRENT_YEAR_TOTAL_TRADES", 5110033m);
        AssertExtensionValue(instrumentDto.ExtensionData, "MOVING_24_HOUR_VOLUME", 448.29185943m);
        AssertExtensionValue(instrumentDto.ExtensionData, "MOVING_24_HOUR_LOW", 54137.73m);
        AssertExtensionValue(instrumentDto.ExtensionData, "MOVING_24_HOUR_CHANGE", 231.18m);
        AssertExtensionValue(instrumentDto.ExtensionData, "MOVING_7_DAY_QUOTE_VOLUME_BUY", 84970972.0590778m);
        AssertExtensionValue(instrumentDto.ExtensionData, "MOVING_7_DAY_TOTAL_TRADES", 265210m);
        AssertExtensionValue(instrumentDto.ExtensionData, "MOVING_7_DAY_CHANGE_PERCENTAGE", 2.80901143951722m);
        AssertExtensionValue(instrumentDto.ExtensionData, "MOVING_30_DAY_VOLUME", 10660.18588067m);
        AssertExtensionValue(instrumentDto.ExtensionData, "MOVING_30_DAY_HIGH", 70257.11m);
        AssertExtensionValue(instrumentDto.ExtensionData, "MOVING_30_DAY_CHANGE", -13176.38m);
        AssertExtensionValue(instrumentDto.ExtensionData, "MOVING_90_DAY_QUOTE_VOLUME", 1799811523.05405m);
        AssertExtensionValue(instrumentDto.ExtensionData, "MOVING_90_DAY_TOTAL_TRADES_SELL", 995074m);
        AssertExtensionValue(instrumentDto.ExtensionData, "MOVING_90_DAY_CHANGE_PERCENTAGE", -12.7012802273682m);
        AssertExtensionValue(instrumentDto.ExtensionData, "MOVING_180_DAY_VOLUME_BUY", 30297.46761899m);
        AssertExtensionValue(instrumentDto.ExtensionData, "MOVING_180_DAY_OPEN", 75155.88m);
        AssertExtensionValue(instrumentDto.ExtensionData, "MOVING_180_DAY_CHANGE", -20646.83m);
        AssertExtensionValue(instrumentDto.ExtensionData, "MOVING_365_DAY_QUOTE_VOLUME", 8516154496.54364m);
        AssertExtensionValue(instrumentDto.ExtensionData, "MOVING_365_DAY_TOTAL_TRADES", 13173068m);
        AssertExtensionValue(instrumentDto.ExtensionData, "MOVING_365_DAY_CHANGE_PERCENTAGE", -40.1570063358268m);
        AssertExtensionValue(instrumentDto.ExtensionData, "LIFETIME_FIRST_TRADE_TS", 1429766376m);
        AssertExtensionValue(instrumentDto.ExtensionData, "LIFETIME_VOLUME", 4894106.92362097m);
        AssertExtensionValue(instrumentDto.ExtensionData, "LIFETIME_QUOTE_VOLUME", 90322883767.3988m);
        AssertExtensionValue(instrumentDto.ExtensionData, "LIFETIME_HIGH_TS", 1759777092m);
        AssertExtensionValue(instrumentDto.ExtensionData, "LIFETIME_TOTAL_TRADES", 107624115m);
        AssertExtensionValue(instrumentDto.ExtensionData, "LIFETIME_CHANGE_PERCENTAGE", 27154.525m);
    }

    [Fact]
    public void MarketTickDto_IsMappedToModelCorrectly()
    {
        // setup
        var tickDto = CreateMockedTickDto();
        
        // act
        var tickModel = tickDto.ToDomainModel();

        // assert
        var instrumentDto = Assert.Single(tickDto.Data!).Value;
        Assert.Equal(instrumentDto.Ccseq, tickModel.Ccseq);
        Assert.Null(tickModel.Note);

        Assert.Equal(instrumentDto.Market, tickModel.Market);
        Assert.Equal(instrumentDto.Instrument, tickModel.Instrument);

        Assert.NotNull(tickModel.BaseCurrency);
        Assert.Equal(instrumentDto.BaseId, tickModel.BaseCurrency.Id);
        Assert.Equal(instrumentDto.Base, tickModel.BaseCurrency.Name);

        Assert.NotNull(tickModel.QuoteCurrency);
        Assert.Equal(instrumentDto.QuoteId, tickModel.QuoteCurrency.Id);
        Assert.Equal(instrumentDto.Quote, tickModel.QuoteCurrency.Name);

        Assert.Equal(instrumentDto.Price, tickModel.Price);
        Assert.Equal(instrumentDto.PriceFlag, tickModel.PriceFlag);
        AssertTimestamp(
            tickModel.PriceUpdateTimestamp,
            instrumentDto.PriceLastUpdateTs,
            instrumentDto.PriceLastUpdateTsNs);

        Assert.NotNull(tickModel.LastTrade);
        Assert.Equal(instrumentDto.LastTradeCcseq, tickModel.LastTrade.Ccseq);
        Assert.Equal(instrumentDto.LastTradeId, tickModel.LastTrade.TradeId);
        Assert.Equal(instrumentDto.LastTradeQuantity, tickModel.LastTrade.Quantity);
        Assert.Equal(instrumentDto.LastTradeQuoteQuantity, tickModel.LastTrade.QuoteQuantity);
        Assert.Null(tickModel.LastTrade.Price);
        Assert.Equal(ParseTradeSide(instrumentDto.LastTradeSide), tickModel.LastTrade.Side);
        AssertTimestamp(
            tickModel.LastTrade.Timestamp,
            instrumentDto.LastTradeReceivedTs,
            instrumentDto.LastTradeReceivedTsNs);
        AssertTimestamp(
            tickModel.LastTrade.ReceivedTimestamp,
            instrumentDto.LastTradeReceivedTs,
            instrumentDto.LastTradeReceivedTsNs);

        Assert.NotNull(tickModel.LastProcessedTrade);
        Assert.Equal(instrumentDto.LastProcessedTradeCcseq, tickModel.LastProcessedTrade.Ccseq);
        Assert.Null(tickModel.LastProcessedTrade.TradeId);
        Assert.Equal(instrumentDto.LastProcessedTradeQuantity, tickModel.LastProcessedTrade.Quantity);
        Assert.Equal(instrumentDto.LastProcessedTradeQuoteQuantity, tickModel.LastProcessedTrade.QuoteQuantity);
        Assert.Equal(instrumentDto.LastProcessedTradePrice, tickModel.LastProcessedTrade.Price);
        Assert.Equal(ParseTradeSide(instrumentDto.LastProcessedTradeSide), tickModel.LastProcessedTrade.Side);
        AssertTimestamp(
            tickModel.LastProcessedTrade.Timestamp,
            instrumentDto.LastProcessedTradeTs,
            instrumentDto.LastProcessedTradeTsNs);
        AssertTimestamp(
            tickModel.LastProcessedTrade.ReceivedTimestamp,
            instrumentDto.LastProcessedTradeReceivedTs,
            instrumentDto.LastProcessedTradeReceivedTsNs);

        AssertOrder(
            tickModel.BestBid,
            instrumentDto.BestBid,
            instrumentDto.BestBidQuantity,
            instrumentDto.BestBidQuoteQuantity,
            instrumentDto.BestBidLastUpdateTs,
            instrumentDto.BestBidLastUpdateTsNs,
            instrumentDto.BestBidPositionInBookUpdateTs,
            instrumentDto.BestBidPositionInBookUpdateTsNs);
        AssertOrder(
            tickModel.BestAsk,
            instrumentDto.BestAsk,
            instrumentDto.BestAskQuantity,
            instrumentDto.BestAskQuoteQuantity,
            instrumentDto.BestAskLastUpdateTs,
            instrumentDto.BestAskLastUpdateTsNs,
            instrumentDto.BestAskPositionInBookUpdateTs,
            instrumentDto.BestAskPositionInBookUpdateTsNs);

        Assert.NotNull(instrumentDto.ExtensionData);
        var ranges = Enum.GetValues<TimeWindowRanges>();
        Assert.Equal(ranges.Length, tickModel.TradeStatistics?.Count);

        var tradeStatistics = tickModel.TradeStatisticsByRange;
        Assert.NotNull(tradeStatistics);
        foreach (var range in ranges)
        {
            Assert.True(tradeStatistics.TryGetValue(range, out var statistics));
            AssertTradeStatistics(statistics, instrumentDto.ExtensionData, range, tickModel.Ccseq);
        }
    }

    private static void AssertTradeStatistics(
        TradeStatistics statistics,
        IDictionary<string, object> extensionData,
        TimeWindowRanges range,
        long tickCcseq)
    {
        var prefix = GetStatisticsPrefix(range);

        Assert.Equal(range, statistics.Range);
        Assert.Equal(tickCcseq, statistics.SourceTickCcseq);
        Assert.Null(statistics.SourceTick);
        Assert.Equal(GetExtensionValue<decimal?>(extensionData, $"{prefix}_VOLUME"), statistics.VolumeTotal);
        Assert.Equal(GetExtensionValue<decimal?>(extensionData, $"{prefix}_VOLUME_BUY"), statistics.VolumeBuy);
        Assert.Equal(GetExtensionValue<decimal?>(extensionData, $"{prefix}_VOLUME_SELL"), statistics.VolumeSell);
        Assert.Equal(GetExtensionValue<decimal?>(extensionData, $"{prefix}_VOLUME_UNKNOWN"), statistics.VolumeUnknown);
        Assert.Equal(GetExtensionValue<decimal?>(extensionData, $"{prefix}_QUOTE_VOLUME"), statistics.QuoteVolumeTotal);
        Assert.Equal(GetExtensionValue<decimal?>(extensionData, $"{prefix}_QUOTE_VOLUME_BUY"), statistics.QuoteVolumeBuy);
        Assert.Equal(GetExtensionValue<decimal?>(extensionData, $"{prefix}_QUOTE_VOLUME_SELL"), statistics.QuoteVolumeSell);
        Assert.Equal(GetExtensionValue<decimal?>(extensionData, $"{prefix}_QUOTE_VOLUME_UNKNOWN"), statistics.QuoteVolumeUnknown);
        Assert.Equal(GetExtensionValue<decimal?>(extensionData, $"{prefix}_OPEN"), statistics.OpenPrice);
        Assert.Equal(GetExtensionValue<decimal?>(extensionData, $"{prefix}_HIGH"), statistics.HighPrice);
        Assert.Equal(GetExtensionValue<decimal?>(extensionData, $"{prefix}_LOW"), statistics.LowPrice);
        Assert.Equal(GetExtensionValue<decimal?>(extensionData, $"{prefix}_CHANGE"), statistics.PriceChange);
        Assert.Equal(GetExtensionValue<decimal?>(extensionData, $"{prefix}_CHANGE_PERCENTAGE"), statistics.PriceChangePercent);
        Assert.Equal(GetExtensionValue<long?>(extensionData, $"{prefix}_TOTAL_TRADES"), statistics.TotalTrades);
        Assert.Equal(GetExtensionValue<long?>(extensionData, $"{prefix}_TOTAL_TRADES_BUY"), statistics.TotalBuys);
        Assert.Equal(GetExtensionValue<long?>(extensionData, $"{prefix}_TOTAL_TRADES_SELL"), statistics.TotalSells);
        Assert.Equal(GetExtensionValue<long?>(extensionData, $"{prefix}_TOTAL_TRADES_UNKNOWN"), statistics.TotalUnknownTrades);
        AssertTimestamp(statistics.HighPriceTimestamp, GetExtensionValue<long?>(extensionData, $"{prefix}_HIGH_TS"));
        AssertTimestamp(statistics.LowPriceTimestamp, GetExtensionValue<long?>(extensionData, $"{prefix}_LOW_TS"));
        AssertTimestamp(statistics.FirstTradeTimestamp, GetExtensionValue<long?>(extensionData, $"{prefix}_FIRST_TRADE_TS"));
    }

    private static string GetStatisticsPrefix(TimeWindowRanges range) => range switch
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
        _ => throw new ArgumentOutOfRangeException(nameof(range), range, null),
    };

    private static T? GetExtensionValue<T>(IDictionary<string, object> extensionData, string propertyName)
    {
        if (!extensionData.TryGetValue(propertyName, out var value))
        {
            return default;
        }

        return Assert.IsType<JsonElement>(value).Deserialize<T>();
    }

    private static void AssertOrder(
        Order? order,
        decimal? price,
        decimal? quantity,
        decimal? quoteQuantity,
        long? lastUpdateSeconds,
        long? lastUpdateNanoseconds,
        long? positionUpdateSeconds,
        long? positionUpdateNanoseconds)
    {
        Assert.NotNull(order);
        Assert.Equal(price, order.Price);
        Assert.Equal(quantity, order.Quantity);
        Assert.Equal(quoteQuantity, order.QuoteQuantity);
        AssertTimestamp(order.LastUpdateTimestamp, lastUpdateSeconds, lastUpdateNanoseconds);
        AssertTimestamp(order.PositionUpdateTimestamp, positionUpdateSeconds, positionUpdateNanoseconds);
    }

    private static void AssertTimestamp(
        MarketTimestamp? timestamp,
        long? expectedSeconds,
        long? expectedNanoseconds = null)
    {
        if (!expectedSeconds.HasValue)
        {
            Assert.Null(timestamp);
            return;
        }

        Assert.NotNull(timestamp);
        var nanoseconds = expectedNanoseconds ?? 0;
        Assert.Equal(expectedSeconds.Value, timestamp.UnixSeconds);
        Assert.Equal(nanoseconds, timestamp.Nanoseconds);
        Assert.Equal(
            DateTime.UnixEpoch.AddSeconds(expectedSeconds.Value).AddTicks(nanoseconds / 100),
            timestamp.Date);
    }

    private static TradeSides ParseTradeSide(string? side) =>
        side?.Equals("SELL", StringComparison.OrdinalIgnoreCase) == true
            ? TradeSides.Sell
            : TradeSides.Buy;

    private static void AssertExtensionValue(
        IDictionary<string, object> extensionData,
        string propertyName,
        decimal expected)
    {
        Assert.True(extensionData.TryGetValue(propertyName, out var value));
        var jsonElement = Assert.IsType<JsonElement>(value);
        Assert.Equal(expected, jsonElement.GetDecimal());
    }
    
    private MarketTickDto CreateMockedTickDto()
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var path = "Mocks/LastSpotTick.json";
        var mockData = File.ReadAllText(Path.Combine(baseDirectory, path));
        return JsonSerializer.Deserialize<MarketTickDto>(mockData) ?? throw new InvalidOperationException();
    }
}
