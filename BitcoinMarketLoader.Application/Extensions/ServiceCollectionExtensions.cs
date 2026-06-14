using BitcoinMarketLoader.Application.Configurations;
using BitcoinMarketLoader.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BitcoinMarketLoader.Application.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplicationServices(IConfiguration config)
        {
            services.AddMemoryCache();
            services.AddScoped<IBitCoinDataService, BitCoinDataService>();
            services.AddScoped<ICzkExchangeRateService, CzkExchangeRateService>();
            services.AddScoped<IModelCurrencyConversionService, ModelCurrencyConversionService>();
            services.AddSingleton<IMarketTickPollingService, MarketTickPollingService>();

            services.Configure<ExchangeRateConfig>(config.GetSection(ExchangeRateConfig.SectionName));
            services.Configure<MarketTickPollingOptions>(
                config.GetSection(MarketTickPollingOptions.SectionName));
            
            return services;
        }
    }
}
