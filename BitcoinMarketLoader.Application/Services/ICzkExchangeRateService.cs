namespace BitcoinMarketLoader.Application.Services;

public interface ICzkExchangeRateService
{
    /// <summary>
    /// Get exchange rate for converting CZK to a given currency on a given date.
    /// </summary>
    /// <param name="targetCurrency">Code of the target currency</param>
    /// <param name="date">Date for which to get the exchange rate. If null, get the latest exchange rate.</param>
    /// <returns>Exchange rate for 1 unit of the target currency</returns>
    Task<decimal> GetExchangeRate(string targetCurrency, DateTime? date = null);
    
    /// <summary>
    /// Convert amount of CZK to a target currency with exchange rate from a given date
    /// </summary>
    /// <param name="targetCurrency">Code of the target currency</param>
    /// <param name="amount">Amount in CZK to convert</param>
    /// <param name="date">Date for which to get the exchange rate. If null, get the latest exchange rate.</param>
    /// <returns>Converted amount in the target currency. Null if input amount is null.</returns>
    Task<decimal?> ConvertFromCzk(string targetCurrency, decimal? amount, DateTime? date = null);

    /// <summary>
    /// Convert amount of a target currency to CZK with exchange rate from a given date
    /// </summary>
    /// <param name="sourceCurrency">Code of the source currency</param>
    /// <param name="amount">Amount in source currency to convert</param>
    /// <param name="date">Date for which to get the exchange rate. If null, get the latest exchange rate.</param>
    /// <returns>Converted amount in CZK. Null if input amount is null.</returns>
    Task<decimal?> ConvertToCzk(string sourceCurrency, decimal? amount, DateTime? date = null);
}