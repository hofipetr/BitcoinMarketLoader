using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace BitcoinMarketLoader.Infrastructure.Http;

public abstract class BaseHttpClient : IBaseHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    
    public BaseHttpClient(HttpClient httpClient, IOptions<BaseHttpClientConfig> options, JsonSerializerOptions? jsonOptions = null)
    {
        _httpClient = httpClient;
        var config = options.Value;

        if (!string.IsNullOrWhiteSpace(config.BaseUrl))
        {
            _httpClient.BaseAddress = new Uri(config.BaseUrl);
        }

        if (config.TimeoutSeconds is > 0)
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds.Value);
        }

        _jsonOptions = jsonOptions ?? new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

        // default Accept header
        if (!_httpClient.DefaultRequestHeaders.Accept.Any())
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }

    private static string BuildQueryString(IDictionary<string, string>? queryParams)
    {
        if (queryParams == null || !queryParams.Any()) return string.Empty;
        var sb = new StringBuilder();
        bool first = true;
        foreach (var kv in queryParams)
        {
            var key = Uri.EscapeDataString(kv.Key);
            var val = Uri.EscapeDataString(kv.Value);
            sb.Append(first ? "?" : "&");
            sb.Append(key).Append("=").Append(val);
            first = false;
        }
        return sb.ToString();
    }

    public async Task<TResult?> GetAsync<TResult>(string path, IDictionary<string, string>? queryParams = null, CancellationToken cancellationToken = default)
    {
        if (path == null) throw new ArgumentNullException(nameof(path));

        var relative = path.StartsWith("/") ? path.TrimStart('/') : path;
        var query = BuildQueryString(queryParams);
        var requestUri = string.IsNullOrEmpty(_httpClient.BaseAddress?.ToString()) ? relative + query : new Uri(_httpClient.BaseAddress, relative + query).ToString();

        using var response = await _httpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(content)) return default;

        var result = JsonSerializer.Deserialize<TResult?>(content, _jsonOptions);
        return result;
    }
    
    public async Task<TResult?> PostAsync<TRequest, TResult>(string path, TRequest requestBody, CancellationToken cancellationToken = default)
    {
        if (path == null) throw new ArgumentNullException(nameof(path));

        var relative = path.StartsWith("/") ? path.TrimStart('/') : path;
        var requestUri = string.IsNullOrEmpty(_httpClient.BaseAddress?.ToString()) ? relative : new Uri(_httpClient.BaseAddress, relative).ToString();

        var payload = JsonSerializer.Serialize(requestBody, _jsonOptions);
        using var content = new StringContent(payload, Encoding.UTF8, "application/json");
        using var response = await _httpClient.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(responseBody)) return default;

        var result = JsonSerializer.Deserialize<TResult?>(responseBody, _jsonOptions);
        return result;
    }
}
