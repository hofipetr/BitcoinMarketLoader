using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using BitcoinMarketLoader.Models;

namespace BitcoinMarketLoader.Services;

public sealed class BitcoinMarketApiClient(HttpClient httpClient)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() },
    };

    public async Task<PagedResponse<MarketTickDto>> GetMarketTicksAsync(
        int page,
        int pageSize,
        string currency,
        CancellationToken cancellationToken = default)
    {
        var requestUri =
            $"btcTicks?page={page}&pageSize={pageSize}&currencyCodes={Uri.EscapeDataString(currency)}";
        var response = await httpClient
            .GetFromJsonAsync<PagedResponse<MarketTickDto>>(
                requestUri,
                JsonOptions,
                cancellationToken)
            .ConfigureAwait(false);

        return response
            ?? throw new InvalidOperationException("The market ticks API returned an empty response.");
    }

    public async Task<int> GetPollingIntervalAsync(
        CancellationToken cancellationToken = default)
    {
        return await httpClient
            .GetFromJsonAsync<int>("polling/interval", cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task UpdateMarketTickNoteAsync(
        long marketTickId,
        string? note,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient
            .PostAsJsonAsync(
                $"btcTicks/{marketTickId}/note",
                new UpdateMarketTickNoteRequest(note),
                JsonOptions,
                cancellationToken)
            .ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"The API returned {(int)response.StatusCode} {response.ReasonPhrase}.");
        }
    }

    private sealed record UpdateMarketTickNoteRequest(string? Note);
}
