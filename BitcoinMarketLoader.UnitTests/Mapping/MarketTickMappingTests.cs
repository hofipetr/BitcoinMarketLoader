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
        
        Assert.Equal("BTC-EUR", instrumentDto.Instrument);
        Assert.Equal("BTC-EUR", instrumentDto.Instrument);

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