using System.Text.Json;
using Microsoft.Extensions.Options;
using BitcoinMarketLoader.Infrastructure.Dtos.CnbApi;

namespace BitcoinMarketLoader.Infrastructure.Http;

public class CnbApiClient(
    HttpClient httpClient,
    IOptions<CnbApiClientConfig> options,
    JsonSerializerOptions? jsonOptions = null)
    : BaseHttpClient(httpClient, options, jsonOptions), ICnbApiClient
{
    private const string DateQueryParameterName = "date";
    private const string LangQueryParameterName = "lang";
    private const string CurrencyQueryParameterName = "currency";
    private const string YearMonthQueryParameterName = "yearMonth";

    public async Task<ExRateDailyResponse> GetDailyExchangeRatesAsync(DateTime? date = null, string? language = null)
    {
        var queryParams = new Dictionary<string, string>();

        if (date.HasValue)
        {
            queryParams[DateQueryParameterName] = date.Value.ToString("yyyy-MM-dd");
        }

        if (!string.IsNullOrWhiteSpace(language))
        {
            queryParams[LangQueryParameterName] = language;
        }

        var result = await GetAsync<ExRateDailyResponse>("/cnbapi/exrates/daily", queryParams).ConfigureAwait(false);
        return result ?? throw new InvalidOperationException("Empty response from CNB API daily exchange rates endpoint");
    }

    public async Task<ExRateDailyCurrencyMonthResponse> GetDailyExchangeRatesForMonthAsync(string currency, DateTime? date = null)
    {
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("currency is required", nameof(currency));

        var queryParams = new Dictionary<string, string>
        {
            [CurrencyQueryParameterName] = currency
        };

        if (date.HasValue)
        {
            queryParams[YearMonthQueryParameterName] = date.Value.ToString("yyyy-MM");
        }

        var result = await GetAsync<ExRateDailyCurrencyMonthResponse>("/cnbapi/exrates/daily-currency-month", queryParams).ConfigureAwait(false);
        return result ?? throw new InvalidOperationException("Empty response from CNB API daily currency month rates endpoint");
    }
}


