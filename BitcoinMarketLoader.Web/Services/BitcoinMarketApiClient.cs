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

    public async Task DeleteMarketTickAsync(
        long marketTickId,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient
            .DeleteAsync($"btcTicks/{marketTickId}", cancellationToken)
            .ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"The API returned {(int)response.StatusCode} {response.ReasonPhrase}.");
        }
    }

    public async Task<int> DeleteMarketTicksAsync(
        IReadOnlyCollection<long> marketTickIds,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, "btcTicks")
        {
            Content = JsonContent.Create(
                new DeleteMarketTicksRequest(marketTickIds),
                options: JsonOptions),
        };
        using var response = await httpClient
            .SendAsync(request, cancellationToken)
            .ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"The API returned {(int)response.StatusCode} {response.ReasonPhrase}.");
        }

        var result = await response.Content
            .ReadFromJsonAsync<DeleteMarketTicksResponse>(
                JsonOptions,
                cancellationToken)
            .ConfigureAwait(false);

        return result?.DeletedCount
            ?? throw new InvalidOperationException("The delete API returned an empty response.");
    }

    private sealed record UpdateMarketTickNoteRequest(string? Note);
    private sealed record DeleteMarketTicksRequest(IReadOnlyCollection<long> Ccseqs);
    private sealed record DeleteMarketTicksResponse(int DeletedCount);
}
