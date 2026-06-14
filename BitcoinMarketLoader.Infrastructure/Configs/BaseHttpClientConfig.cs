namespace BitcoinMarketLoader.Infrastructure.Http;

public abstract class BaseHttpClientConfig
{
    /// <summary>
    /// Base URL of the target server (including scheme), e.g. https://api.example.com
    /// </summary>
    public required string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// Optional default request timeout in seconds. Used only if it is > 0. The HttpClient default timeout is used otherwise.
    /// </summary>
    public int? TimeoutSeconds { get; set; }
}
