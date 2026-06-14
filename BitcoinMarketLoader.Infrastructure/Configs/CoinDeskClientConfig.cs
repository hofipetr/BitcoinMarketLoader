namespace BitcoinMarketLoader.Infrastructure.Http;

public class CoinDeskClientConfig: BaseHttpClientConfig
{
    public const string Name = "CoinDeskClient";

    public string? ApiKey { get; init; }

    /// <summary>
    /// Flag for testing - overriding MarketTick ccseq so that mocked ticks have unique id
    /// </summary>
    public bool OverrideTickId { get; init; } = false;
}