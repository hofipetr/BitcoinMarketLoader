using BitcoinMarketLoader.Application.Services;
using BitcoinMarketLoader.Domain.Enums;
using BitcoinMarketLoader.Domain.Extensions;
using BitcoinMarketLoader.Domain.Market;
using BitcoinMarketLoader.UnitTests.Mocks;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace BitcoinMarketLoader.UnitTests.Services;

public class ModelCurrencyConversionServiceTests
{
    const decimal DefaultExchangeRate = 33m;
    const string ExchangeCurrency = "EUR";
    private static readonly DateTime TestDate = new DateTime(2026, 5, 28, 16, 30, 0, DateTimeKind.Utc);
    private static readonly DateTime LifetimeRangeStartDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    
    /// <summary>
    /// Exchange rate for mocked <see cref="ICzkExchangeRateService"/>
    /// </summary>
    private static decimal RateForDate(DateTime date) => ((int)(TestDate - date).TotalDays) + 10m;
    
    private readonly ICzkExchangeRateService _czkExchangeRateService = Substitute.For<ICzkExchangeRateService>();
    
    private readonly IModelCurrencyConversionService _sut;

    public ModelCurrencyConversionServiceTests()
    {
        _czkExchangeRateService.ConvertToCzk(Arg.Any<string>(), Arg.Any<decimal?>(), Arg.Any<DateTime?>())
            .Returns(ci =>
            {
                decimal? sourceAmount = ci.ArgAt<decimal?>(1);
                if (sourceAmount.HasValue)
                {
                    DateTime sourceDate = ci.ArgAt<DateTime?>(2) ?? TestDate;
                    var exRate = RateForDate(sourceDate);
                    return sourceAmount.Value * exRate;
                }
                return null;
            });
        
        _sut = new ModelCurrencyConversionService(
            _czkExchangeRateService,
            NullLogger<ModelCurrencyConversionService>.Instance);
    }

    [Fact]
    public async Task ModelCurrencyConversionService_convertsMarketTickToCzk()
    {
        // arrange
        var marketTick = MockHelper.CreateMockedInstance<MarketTick>(false);
        marketTick.GeneratedTimestamp = TestDate;
        marketTick.PriceUpdateTimestamp = new MarketTimestamp(TestDate);
        marketTick.QuoteCurrency = new MarketCurrency { Name = "EUR" };
        marketTick.BestAsk = MockHelper.CreateMockedInstance<Order>(false, o => o.LastUpdateTimestamp = new MarketTimestamp(TestDate.AddDays(-1)));
        marketTick.BestBid = MockHelper.CreateMockedInstance<Order>(false);
        marketTick.LastTrade = MockHelper.CreateMockedInstance<TradeDetails>(false, lt => lt.Timestamp =  new MarketTimestamp(TestDate.AddDays(-5)));
        marketTick.LastProcessedTrade = MockHelper.CreateMockedInstance<TradeDetails>(false, lt => lt.ReceivedTimestamp =  new MarketTimestamp(TestDate.AddDays(-6)));
        marketTick.TradeStatistics = Enum.GetValues<TimeWindowRanges>()
            .Select(r => MockHelper.CreateMockedInstance<TradeStatistics>(false, ts =>
            {
                ts.Range = r;
                ts.SourceTick = marketTick;
                ts.SourceTickCcseq = marketTick.Ccseq;
                if (r == TimeWindowRanges.LifeTime)
                {
                    ts.FirstTradeTimestamp = new MarketTimestamp(LifetimeRangeStartDate);
                    ts.LowPriceTimestamp = new MarketTimestamp(LifetimeRangeStartDate.AddDays(50));
                    ts.HighPriceTimestamp = new MarketTimestamp(LifetimeRangeStartDate.AddDays(100));
                }
            }))
            .ToArray();
        
        // act
        var conversionSuccess = await _sut.ConvertEurToCzk(marketTick);
        
        // assert
        // REM: All source numeric values are 1. Converted values should be equal to the exchange rate for the relevant date. 
        Assert.True(conversionSuccess);
        Assert.Equal("CZK", marketTick.QuoteCurrency.Name);
        Assert.Equal(marketTick.Price, RateForDate(marketTick.PriceUpdateTimestamp.Date));
        Assert.Equal(marketTick.BestAsk.Price, RateForDate(marketTick.BestAsk.LastUpdateTimestamp!.Date));
        // BestBid with undefined timestamp should fall back to default date
        Assert.Equal(marketTick.BestBid.Price, RateForDate(TestDate)); 
        Assert.Equal(marketTick.LastTrade.Price, RateForDate(marketTick.LastTrade.Timestamp!.Date));
        // LastProcessedTrade with undefined Timestamp should fall back to ReceivedTimestamp
        Assert.Equal(marketTick.LastProcessedTrade.Price, RateForDate(marketTick.LastProcessedTrade.ReceivedTimestamp!.Date)); // BestBid with undefined timestamp should have felt back to default date

        foreach (var statistics in marketTick.TradeStatistics)
        {
            CheckTradeStatistics(statistics);
        }
    }

    private static void CheckTradeStatistics(TradeStatistics statistics)
    {
        var rangeStart = statistics.Range switch
        {
            TimeWindowRanges.LifeTime => statistics.FirstTradeTimestamp!.Date,
            _ => statistics.Range.RangeStart(TestDate)
        };
        Assert.Equal(statistics.OpenPrice, RateForDate(rangeStart));
        Assert.Equal(statistics.QuoteVolumeTotal, RateForDate(TestDate));
        Assert.Equal(statistics.QuoteVolumeBuy, RateForDate(TestDate));
        Assert.Equal(statistics.QuoteVolumeSell, RateForDate(TestDate));
        Assert.Equal(statistics.QuoteVolumeUnknown, RateForDate(TestDate));

        var priceChange = statistics.SourceTick!.Price - statistics.OpenPrice;
        Assert.Equal(statistics.PriceChange, priceChange);
        Assert.Equal(statistics.PriceChangePercent, (priceChange / statistics.OpenPrice) * 100m);
    }
}