namespace BitcoinMarketLoader.Infrastructure.Http;

public class CoinDeskClientConfig: BaseHttpClientConfig
{
    public const string Name = "CoinDeskClient";

    public string? ApiKey { get; set; }
}