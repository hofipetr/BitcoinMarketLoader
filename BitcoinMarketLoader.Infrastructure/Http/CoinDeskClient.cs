using System.Net.Http.Headers;
using System.Text.Json;
using BitcoinMarketLoader.Infrastructure.Dtos.MarketTickDtos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BitcoinMarketLoader.Infrastructure.Http;

public class CoinDeskClient: BaseHttpClient, ICoinDeskClient
{
    private const string MarketQueryParameterName = "market";
    private const string InstrumentsQueryParameterName = "instruments";

    private readonly CoinDeskClientConfig _config;
    private readonly ILogger<CoinDeskClient> _logger;
    
    public CoinDeskClient(
        HttpClient httpClient,
        IOptions<CoinDeskClientConfig> options,
        ILogger<CoinDeskClient> logger,
        JsonSerializerOptions? jsonOptions = null): base(httpClient, options, jsonOptions)
    {
        _logger = logger;
        _config = options.Value;
        if (!string.IsNullOrWhiteSpace(_config.ApiKey))
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", options.Value.ApiKey);
        }

        if (_config.OverrideTickId)
        {
            _logger.LogWarning("Client is configured to override MarketTick ccseq!");
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
        
        if (_config.OverrideTickId && result?.Data?.Count > 0)
        {
            // updating mocked ticks to be unique
            var timestamp = DateTime.UtcNow.Ticks;
            foreach (var item in result.Data.Values)
            {
                item.Ccseq = timestamp++;
            }
        }
        
        return result ?? throw new InvalidOperationException("Empty response from CoinDesk latest tick endpoint");
    }
}
