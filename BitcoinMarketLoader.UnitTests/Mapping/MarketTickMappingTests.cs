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
        
        // Validate identity/mapping
        Assert.Equal("BTC-EUR", instrumentDto.Instrument);
        Assert.Equal("BTC-EUR", instrumentDto.MappedInstrument);
        Assert.Equal("BTC", instrumentDto.Base);
        Assert.Equal("EUR", instrumentDto.Quote);
        Assert.Equal(107656825L, instrumentDto.Ccseq);

        // Core price values
        Assert.Equal(54509.05m, instrumentDto.Price);
        Assert.Equal("DOWN", instrumentDto.PriceFlag);
        Assert.Equal(54507.28m, instrumentDto.BestBid);
        Assert.Equal(54509.05m, instrumentDto.BestAsk);

        // Last processed trade
        Assert.Equal(54509.05m, instrumentDto.LastProcessedTradePrice);
        Assert.Equal(0.00272397m, instrumentDto.LastProcessedTradeQuantity);

        var model = tickDto.ToDomainModel();
        var data = tickDto.Data; 
        
        Assert.Equal(tickDto.Data, data);
    }
    
    [Fact]
    public void FilledDtoIsMapped()
    {
        var dto = CreateMockedTickDto();
        Assert.NotNull(dto.Data);

        var model = dto.ToDomainModel();
        var data = dto.Data; 
        
        Assert.Equal(dto.Data);
    }
    
    private MarketTickDto CreateMockedTickDto()
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var path = "Mocks/LastTickSpot.json";
        var mockData = File.ReadAllText(Path.Combine(baseDirectory, path));
        return JsonSerializer.Deserialize<MarketTickDto>(mockData) ?? throw new InvalidOperationException();
    }
}