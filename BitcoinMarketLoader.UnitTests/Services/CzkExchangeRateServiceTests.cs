using BitcoinMarketLoader.Application.Configurations;
using BitcoinMarketLoader.Application.Services;
using BitcoinMarketLoader.Infrastructure.Dtos.CnbApi;
using BitcoinMarketLoader.Infrastructure.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace BitcoinMarketLoader.UnitTests.Services;

public class CzkExchangeRateServiceTests
{
    private readonly ICnbApiClient _cnbApiClient = Substitute.For<ICnbApiClient>();
    private readonly IMemoryCache _memoryCache = Substitute.For<IMemoryCache>();
    private readonly IOptions<ExchangeRateConfig> _options = Substitute.For<IOptions<ExchangeRateConfig>>();
    private readonly ICacheEntry _cacheEntry = Substitute.For<ICacheEntry>();

    private readonly ICzkExchangeRateService _sut;

    private const string CachedCurrency = "EUR";
    private const string NotCachedCurrency = "USD";
    private const double CachedRate = 25;
    private const double ApiRate = 20;
    private readonly DateTime _testDate = new DateTime(2026, 5, 1);
    
    public CzkExchangeRateServiceTests()
    {
        var config = new ExchangeRateConfig
        {
            CacheDurationInHours = 1,
            CurrentDayCacheDurationInMinutes = 1
        };
        _options.Value.Returns(config);

        _memoryCache
            .CreateEntry(Arg.Any<object>())
            .Returns(_cacheEntry);
        
        _memoryCache.TryGetValue(Arg.Is<object>(o => ((string)o).Contains(NotCachedCurrency)), out Arg.Any<object?>())
            .Returns(false);

        _memoryCache.TryGetValue(Arg.Is<object>(o => ((string)o).Contains(CachedCurrency)), out Arg.Any<object?>())
            .Returns(ci => 
            {
                ci[1] = (decimal)CachedRate;
                return true;
            });

        _cnbApiClient.GetDailyExchangeRatesAsync(Arg.Any<DateTime?>(), Arg.Any<string?>())
            .Returns(new ExRateDailyResponse
            {
                Rates =
                [
                    new ExRateDailyRest
                    {
                        Amount = 1,
                        Currency = CachedCurrency,
                        CurrencyCode = CachedCurrency,
                        Rate = (double)ApiRate,
                        ValidFor = _testDate
                    },
                    new ExRateDailyRest
                    {
                        Amount = 1,
                        Currency = NotCachedCurrency,
                        CurrencyCode = NotCachedCurrency,
                        Rate = (double)ApiRate,
                        ValidFor = _testDate
                    },
                ]
            });
        
        _sut = new CzkExchangeRateService(
            _cnbApiClient,
            _memoryCache,
            _options,
            new NullLogger<CzkExchangeRateService>());
    }
    
    [Fact]
    public async Task GetExchangeRate_ShouldCallApiClientIfRateNotCached()
    {
        // act
        var rate = await _sut.GetExchangeRate(NotCachedCurrency, _testDate);
        
        // assert
        Assert.Equal((decimal)ApiRate, rate);

        _cnbApiClient.Received().GetDailyExchangeRatesAsync(_testDate, Arg.Any<string?>());
        _memoryCache.Received().CreateEntry(Arg.Is<object>(o => ((string)o).Contains(NotCachedCurrency)));
    }
    
    [Fact]
    public async Task GetExchangeRate_ShouldNotCallApiClientIfRateIsCached()
    {
        // act
        var rate = await _sut.GetExchangeRate(CachedCurrency, _testDate);
        
        // assert
        Assert.Equal((decimal)CachedRate, rate);

        _cnbApiClient.DidNotReceive().GetDailyExchangeRatesAsync(Arg.Any<DateTime?>(), Arg.Any<string?>());
        _memoryCache.DidNotReceive().CreateEntry(Arg.Any<object>());
    }

    [Theory]
    [InlineData(NotCachedCurrency, ApiRate)]
    [InlineData(CachedCurrency, CachedRate)]
    public async Task ConvertFromCzk_ConvertsAmount(string currency, decimal expectedRate)
    {
        // arrange
        const decimal amount = 100m;
        var expectedResult = amount / expectedRate;
        
        // act
        var converted = await _sut.ConvertFromCzk(currency, amount, _testDate);
        
        // assert
        Assert.Equal(expectedResult, converted);
    }

    [Theory]
    [InlineData(NotCachedCurrency, ApiRate)]
    [InlineData(CachedCurrency, CachedRate)]
    public async Task ConvertToCzk_ConvertsAmount(string currency, decimal expectedRate)
    {
        // arrange
        const decimal amount = 100m;
        var expectedResult = amount * expectedRate;
        
        // act
        var converted = await _sut.ConvertToCzk(currency, amount, _testDate);
        
        // assert
        Assert.Equal(expectedResult, converted);
    }

}