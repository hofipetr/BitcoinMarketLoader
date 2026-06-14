using BitcoinMarketLoader.Application.Configurations;
using Microsoft.Extensions.Caching.Memory;
using BitcoinMarketLoader.Infrastructure.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BitcoinMarketLoader.Application.Services;

public class CzkExchangeRateService(
    ICnbApiClient client,
    IMemoryCache memoryCache,
    IOptions<ExchangeRateConfig> options,
    ILogger<CzkExchangeRateService> logger) : ICzkExchangeRateService
{
    private ExchangeRateConfig Config => options.Value;
    
    public async Task<decimal> GetExchangeRate(string targetCurrency, DateTime? date = null)
    {
        if (string.IsNullOrWhiteSpace(targetCurrency))
            throw new ArgumentException("targetCurrency is required", nameof(targetCurrency));

        var cacheKey = BuildCacheKey(targetCurrency, date);

        if (memoryCache.TryGetValue(cacheKey, out decimal cachedRate))
        {
            logger.LogTrace("Returning exchange rate {rate} for {key} from cache", cachedRate, cacheKey);
            return cachedRate;
        }

        var response = await client.GetDailyExchangeRatesAsync(date).ConfigureAwait(false);

        var rateData = response.Rates
            ?.FirstOrDefault(r => r.CurrencyCode?.Equals(targetCurrency, StringComparison.OrdinalIgnoreCase) == true);
        
        if (rateData?.Rate is null)
            throw new InvalidOperationException($"Exchange rate not found for currency: {targetCurrency}");
        
        var rate = rateData.Rate / (rateData.Amount ?? 1);

        var decimalRate = Convert.ToDecimal(rate);
        var isMoreThanDay = date.HasValue && date.Value.AddDays(1).Date < DateTime.Now; 
        var expiration = isMoreThanDay ? Config.CacheDuration : Config.CurrentDayCacheDuration;
        memoryCache.Set(cacheKey, decimalRate, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        });

        logger.LogTrace("Received exchange rate {rate} for {key} from server. Caching for {duration} minutes", decimalRate, cacheKey, expiration.TotalMinutes);
        return decimalRate;
    }

    public async Task<decimal?> ConvertFromCzk(string targetCurrency, decimal? amount, DateTime? date = null)
    {
        if (string.IsNullOrWhiteSpace(targetCurrency))
            throw new ArgumentException("targetCurrency is required", nameof(targetCurrency));

        var exchangeRate = await GetExchangeRate(targetCurrency, date).ConfigureAwait(false);
        return amount / exchangeRate;
    }

    public async Task<decimal?> ConvertToCzk(string sourceCurrency, decimal? amount, DateTime? date = null)
    {
        if (string.IsNullOrWhiteSpace(sourceCurrency))
            throw new ArgumentException("sourceCurrency is required", nameof(sourceCurrency));

        var exchangeRate = await GetExchangeRate(sourceCurrency, date).ConfigureAwait(false);
        return amount * exchangeRate;
    }

    private static string BuildCacheKey(string targetCurrency, DateTime? date)
    {
        date ??= DateTime.Now;
        return $"exrate:{targetCurrency}:{date:yyyy-MM-dd}";
    }
}
