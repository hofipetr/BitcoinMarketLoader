using BitcoinMarketLoader.Infrastructure.Dtos.CnbApi;

namespace BitcoinMarketLoader.Infrastructure.Http;

public interface ICnbApiClient: IBaseHttpClient
{
    /// <summary>
    /// Get daily exchange rate of CZK to all currencies
    /// </summary>
    /// <param name="date">If specified, get rates valid for the given date. Otherwise, get the latest rates</param>
    /// <param name="language">Optional languge - valid values are "EN" and "CZ" (default)</param>
    /// <returns><see cref="ExRateDailyResponse"/> instance</returns>
    Task<ExRateDailyResponse> GetDailyExchangeRatesAsync(DateTime? date = null, string? language = null);
    
    /// <summary>
    /// Get daily exchange rates of CZK to a specific currency for the whole month.
    /// </summary>
    /// <param name="currency">Currency code</param>
    /// <param name="date">If specified, get rates valid for the month of the given date. Otherwise, get rates for the current month</param>
    /// <returns><see cref="ExRateDailyCurrencyMonthResponse"/></returns>
    Task<ExRateDailyCurrencyMonthResponse> GetDailyExchangeRatesForMonthAsync(string currency, DateTime? date = null);
}