using BitcoinMarketLoader.Application.Enums;
using BitcoinMarketLoader.Application.Services;
using BitcoinMarketLoader.Domain.Market;
using BitcoinMarketLoader.Infrastructure.Databases;
using BitcoinMarketLoader.Infrastructure.Databases.InMemory;
using BitcoinMarketLoader.Infrastructure.Http;
using BitcoinMarketLoader.UnitTests.Mocks;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace BitcoinMarketLoader.UnitTests.Services;

public class BitCoinDataServiceTests
{
    private readonly IMarketRepository _repository = new InMemoryMarketRepository([MockHelper.CreateMarketTick(1)]);
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
        var tick = MockHelper.CreateMarketTickDto(Random.Shared.Next());
        _coinDeskClient.GetLatestSpotTickAsync(Arg.Any<string>(), Arg.Any<ICollection<string>>())
            .Returns(tick);
        
        // Act
        var result = await _sut.FetchLatestBtcMarketTickAsync(CurrencyCodes.CZK);

        // Assert
        Assert.NotNull(tick.Data);
        Assert.Contains(MockHelper.DefaultInstrumentName, tick.Data);
        var btcData = tick.Data[MockHelper.DefaultInstrumentName];

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
    public async Task DeleteMarketTick_RemovesPersistedTick()
    {
        // arrange
        var tick = MockHelper.CreateMarketTick(Random.Shared.Next());
        await _repository.AddMarketTick(tick);
        
        // act
        var fetchedBeforeDelete = await _sut.GetBtcMarketTickAsync(tick.Ccseq, CurrencyCodes.EUR);
        var deletedTick = await _sut.DeleteBtcMarketTickAsync(tick.Ccseq);
        var fetchedAfterDelete = await _sut.GetBtcMarketTickAsync(tick.Ccseq, CurrencyCodes.EUR);
        var deletedTickAgain = await _sut.DeleteBtcMarketTickAsync(tick.Ccseq);
        
        // assert
        Assert.NotNull(fetchedBeforeDelete);
        Assert.True(deletedTick);
        Assert.Null(fetchedAfterDelete);
        Assert.False(deletedTickAgain);
    }

    [Fact]
    public async Task DeleteMarketTicks_ReturnsNumberOfRemovedTicks()
    {
        // arrange
        await _repository.AddMarketTick(MockHelper.CreateMarketTick(2));
        await _repository.AddMarketTick(MockHelper.CreateMarketTick(3));
        await _repository.AddMarketTick(MockHelper.CreateMarketTick(4));

        // act
        var deletedCount = await _sut.DeleteBtcMarketTicksAsync([2, 3, 999]);
        var fetchedTick2 = await _sut.GetBtcMarketTickAsync(2, CurrencyCodes.EUR);
        var fetchedTick3 = await _sut.GetBtcMarketTickAsync(3, CurrencyCodes.EUR);
        var fetchedTick4 = await _sut.GetBtcMarketTickAsync(4, CurrencyCodes.EUR);

        // assert
        Assert.Equal(2, deletedCount);
        Assert.Null(fetchedTick2); // should be deleted
        Assert.Null(fetchedTick3); // should be deleted
        Assert.NotNull(fetchedTick4); // should remain in DB
    }
}
