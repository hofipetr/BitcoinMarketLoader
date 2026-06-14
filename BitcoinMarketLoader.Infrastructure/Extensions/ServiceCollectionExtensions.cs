using BitcoinMarketLoader.Infrastructure.Databases;
using BitcoinMarketLoader.Infrastructure.Databases.InMemory;
using BitcoinMarketLoader.Infrastructure.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BitcoinMarketLoader.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructureServices(IConfiguration config)
        {
            var useInMemoryRepos = config["useInMemoryRepositories"] == "true";
            if (useInMemoryRepos)
            {
                services.AddSingleton<IMarketRepository, InMemoryMarketRepository>();
            }
            else
            {
                var connectionString = config.GetConnectionString("SqlServer");
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException(
                        "Connection string 'SqlServer' is required when in-memory repositories are disabled.");
                }

                services.AddDbContext<MainDbContext>(options =>
                    options.UseSqlServer(connectionString));
                services.AddScoped<IMarketRepository, MarketRepository>();
            }

            services.Configure<CoinDeskClientConfig>(config.GetSection(CoinDeskClientConfig.Name));
            services.Configure<CnbApiClientConfig>(config.GetSection(CnbApiClientConfig.Name));

            services.AddHttpClient();
            services.AddScoped<ICnbApiClient, CnbApiClient>();
            services.AddScoped<ICoinDeskClient, CoinDeskClient>();
            
            return services;
        }
    }

    private static void ConfigureHttpClient(
        HttpClient httpClient,
        BaseHttpClientConfig config)
    {
        httpClient.BaseAddress = new Uri(config.BaseUrl, UriKind.Absolute);

        if (config.TimeoutSeconds is > 0)
        {
            httpClient.Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds.Value);
        }
    }

    private static bool IsValidBaseUrl(string baseUrl) =>
        Uri.TryCreate(baseUrl, UriKind.Absolute, out var uri)
        && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
}
