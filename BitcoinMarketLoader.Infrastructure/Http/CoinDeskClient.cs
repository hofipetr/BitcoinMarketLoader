using System.Net.Http.Headers;
using System.Text.Json;
using BitcoinMarketLoader.Infrastructure.Dtos.MarketTickDtos;
using Microsoft.Extensions.Options;

namespace BitcoinMarketLoader.Infrastructure.Http;

public class CoinDeskClient: BaseHttpClient, ICoinDeskClient
{
    private const string MarketQueryParameterName = "market";
    private const string InstrumentsQueryParameterName = "instruments";
    
    public CoinDeskClient(
        HttpClient httpClient,
        IOptions<CoinDeskClientConfig> options,
        JsonSerializerOptions? jsonOptions = null): base(httpClient, options, jsonOptions)
    {
        if (!string.IsNullOrWhiteSpace(options.Value.ApiKey))
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", options.Value.ApiKey);
        }
    }
    
    public async Task<MarketTickDto> GetLatestSpotTickAsync(string market, ICollection<string> instruments)
    {
        if (string.IsNullOrWhiteSpace(market)) throw new ArgumentException("market is required", nameof(market));
        if (instruments == null || instruments.Count == 0) throw new ArgumentException("instruments is required", nameof(instruments));

        var instrumentsCsv = string.Join(',', instruments);
        var query = new Dictionary<string, string>
        {
            [MarketQueryParameterName] = market,
            [InstrumentsQueryParameterName] = instrumentsCsv
        };

        var result = await GetAsync<MarketTickDto>("/spot/v1/latest/tick", query).ConfigureAwait(false);
        return result ?? throw new InvalidOperationException("Empty response from CoinDesk latest tick endpoint");
    }
}
