using BitcoinMarketLoader.Infrastructure.Dtos.MarketTickDtos;

namespace BitcoinMarketLoader.Infrastructure.Http;

public interface ICoinDeskClient: IBaseHttpClient
{
    /// <summary>
    /// Get latest tick for digital asset valuations
    /// </summary>
    /// <param name="market">Market name (see https://developers.coindesk.com/documentation/data-api/spot_v1_latest_tick for allowed values) </param>
    /// <param name="instruments">Collection of instrument names</param>
    /// <returns><see cref="MarketTickDto"/> instance on success. Throws an exception on error.</returns>
    Task<MarketTickDto> GetLatestSpotTickAsync(string market, ICollection<string> instruments);
}