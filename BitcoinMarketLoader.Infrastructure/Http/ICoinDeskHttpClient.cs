using BitcoinMarketLoader.Infrastructure.Dtos.MarketTickDtos;

namespace BitcoinMarketLoader.Infrastructure.Http;

public interface MarketHttpClient
{
    async Task<MarketTickDto> GetCurrentSpotTickAsync(string symbol);
}