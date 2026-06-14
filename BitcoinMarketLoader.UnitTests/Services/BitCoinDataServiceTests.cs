using BitcoinMarketLoader.Application.Enums;
using BitcoinMarketLoader.Application.Services;
using BitcoinMarketLoader.Domain.Market;
using BitcoinMarketLoader.Infrastructure.Databases;
using BitcoinMarketLoader.Infrastructure.Databases.InMemory;
using BitcoinMarketLoader.Infrastructure.Dtos.MarketTickDtos;
using BitcoinMarketLoader.Infrastructure.Http;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace BitcoinMarketLoader.UnitTests.Services;

public class BitCoinDataServiceTests
{
    private readonly IMarketRepository _repository = new InMemoryMarketRepository([CreateMarketTick(1)]);
    private readonly IModelCurrencyConversionService _modelCurrencyConversionService = Substitute.For<IModelCurrencyConversionService>();
    private readonly ICoinDeskClient _coinDeskClient = Substitute.For<ICoinDeskClient>();
    
    private readonly IBitCoinDataService _sut;

    public BitCoinDataServiceTests()
    {
        _sut = new BitCoinDataService(
            NullLogger<BitCoinDataService>.Instance,
            _coinDeskClient,
            _repository,
            _modelCurrencyConversionService);

        _modelCurrencyConversionService.ConvertEurToCzk(Arg.Any<MarketTick>()).Returns(true);
    }

    [Theory]
    [InlineData(CurrencyCodes.CZK)]
    [InlineData(CurrencyCodes.EUR)]
    public async Task GetMarketTickConvertsOnlyWhenCzkIsRequested(CurrencyCodes requestedCurrencyCodes)
    {
        // Act
        var tick = await _sut.GetBtcMarketTickAsync(1, requestedCurrencyCodes);

        // Assert
        Assert.NotNull(tick?.QuoteCurrency);
        if (requestedCurrencyCodes == CurrencyCodes.EUR)
        {
            _modelCurrencyConversionService.DidNotReceive().ConvertEurToCzk(Arg.Any<MarketTick>());
            
        }
        else
        {
            _modelCurrencyConversionService.Received().ConvertEurToCzk(Arg.Any<MarketTick>());
        }
    }

    [Fact]
    public async Task FetchLatestPersistsEurBeforeConvertingResponseToCzk()
    {
        // Arrange
        var tick = CreateMarketTickDto(Random.Shared.Next());
        _coinDeskClient.GetLatestSpotTickAsync(Arg.Any<string>(), Arg.Any<ICollection<string>>())
            .Returns(tick);
        
        // Act
        var result = await _sut.FetchLatestBtcMarketTickAsync(CurrencyCodes.CZK);

        // Assert
        Assert.NotNull(tick.Data);
        Assert.Contains("BTC-EUR", tick.Data);
        var btcData = tick.Data["BTC-EUR"];

        Assert.NotNull(result);
        Assert.Equal(btcData.Ccseq, result.Ccseq);

        var persisted = await _repository.GetMarketTick(result.Ccseq);
        Assert.NotNull(persisted);
        Assert.Equal(btcData.Quote, persisted.QuoteCurrency?.Name);
        Assert.Equal(btcData.Price, persisted.Price);

        _modelCurrencyConversionService.Received()
            .ConvertEurToCzk(Arg.Is<MarketTick>(t => t.Ccseq == btcData.Ccseq && t.Price == btcData.Price));
    }

    [Fact]
    public async Task DeleteMarketTickRemovesPersistedTick()
    {
        Assert.True(await _sut.DeleteBtcMarketTickAsync(1));
        Assert.Null(await _repository.GetMarketTick(1));
        Assert.False(await _sut.DeleteBtcMarketTickAsync(1));
    }

    [Fact]
    public async Task DeleteMarketTicksReturnsNumberOfRemovedTicks()
    {
        await _repository.AddMarketTick(CreateMarketTick(2));
        await _repository.AddMarketTick(CreateMarketTick(3));

        var deletedCount = await _sut.DeleteBtcMarketTicksAsync([1, 3, 999]);

        Assert.Equal(2, deletedCount);
        Assert.NotNull(await _repository.GetMarketTick(2));
    }

    private static MarketTick CreateMarketTick(long ccseq) =>
        new()
        {
            Ccseq = ccseq,
            GeneratedTimestamp = DateTime.UtcNow,
            Market = "coinbase",
            Instrument = "BTC-EUR",
            BaseCurrency = new MarketCurrency { Id = 1, Name = "BTC" },
            QuoteCurrency = new MarketCurrency { Id = 2, Name = "EUR" },
            Price = 10m,
        };

    private static MarketTickDto CreateMarketTickDto(long ccseq) =>
        new()
        {
            Data = new Dictionary<string, InstrumentDataDto>
            {
                ["BTC-EUR"] = new()
                {
                    Ccseq = ccseq,
                    Market = "coinbase",
                    Instrument = "BTC-EUR",
                    Base = "BTC",
                    BaseId = 1,
                    Quote = "EUR",
                    QuoteId = 2,
                    Price = 10m,
                },
            },
        };
}
