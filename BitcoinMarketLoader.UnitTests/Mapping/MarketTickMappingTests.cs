using System.Text.Json;
using BitcoinMarketLoader.Infrastructure.Dtos.MarketTickDtos;
using BitcoinMarketLoader.Infrastructure.Http;

namespace BitcoinMarketLoader.UnitTests.Mapping;

public class MarketTickMappingTests
{
    [Fact]
    public void DtoIsParsedFromJson()
    {
        var tickDto = CreateMockedTickDto();
        Assert.NotNull(tickDto.Data);
        Assert.Single(tickDto.Data);
        Assert.Contains("BTC-EUR", tickDto.Data);
        var instrumentDto = tickDto.Data["BTC-EUR"]; 
        
        // Basic identity/mapping values from LastSpotTick.json
        Assert.Equal("953", instrumentDto.Type);
        Assert.Equal("coinbase", instrumentDto.Market);
        Assert.Equal("BTC-EUR", instrumentDto.Instrument);
        Assert.Equal("BTC-EUR", instrumentDto.MappedInstrument);
        Assert.Equal("BTC", instrumentDto.Base);
        Assert.Equal("EUR", instrumentDto.Quote);
        Assert.Equal(1L, instrumentDto.BaseId);
        Assert.Equal(1748L, instrumentDto.QuoteId);
        Assert.Equal(107656825L, instrumentDto.Ccseq);

        // Core price values
        Assert.Equal(54509.05m, instrumentDto.Price);
        Assert.Equal("DOWN", instrumentDto.PriceFlag);
        Assert.Equal(1781251001L, instrumentDto.PriceLastUpdateTs);

        // Last trade
        Assert.Equal(0.00272397m, instrumentDto.LastTradeQuantity);
        Assert.Equal("107624222", instrumentDto.LastTradeId);

        // Last processed trade
        Assert.Equal(54509.05m, instrumentDto.LastProcessedTradePrice);
        Assert.Equal(0.00272397m, instrumentDto.LastProcessedTradeQuantity);

        // Top of book
        Assert.Equal(54507.28m, instrumentDto.BestBid);
        Assert.Equal(0.01504342m, instrumentDto.BestBidQuantity);
        Assert.Equal(54509.05m, instrumentDto.BestAsk);

        var model = tickDto.ToDomainModel();
        var data = tickDto.Data; 
    }
    
    private MarketTickDto CreateMockedTickDto()
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var path = "Mocks/LastSpotTick.json";
        var mockData = File.ReadAllText(Path.Combine(baseDirectory, path));
        return JsonSerializer.Deserialize<MarketTickDto>(mockData) ?? throw new InvalidOperationException();
    }
}