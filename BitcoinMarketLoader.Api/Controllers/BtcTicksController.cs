using BitcoinMarketLoader.Application.Enums;
using BitcoinMarketLoader.Application.Models;
using BitcoinMarketLoader.Application.Services;
using BitcoinMarketLoader.Domain.Market;
using Microsoft.AspNetCore.Mvc;

namespace BitcoinMarketLoader.Api.Controllers;

[ApiController]
[Route("btcTicks")]
public class BtcTicksController(IBitCoinDataService bitCoinDataService) : ControllerBase
{
    private const CurrencyCodes DefaultCurrency = CurrencyCodes.CZK;

    /// <summary>
    /// Gets a page of persisted BTC market ticks in the requested currency.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<PagedResponse<MarketTick>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResponse<MarketTick>>> GetBtcMarketTicksAsync(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] CurrencyCodes currencyCodes = DefaultCurrency)
    {
        if (page < 1)
        {
            ModelState.AddModelError(nameof(page), "Page must be greater than zero.");
        }

        if (pageSize < 1)
        {
            ModelState.AddModelError(nameof(pageSize), "Page size must be greater than zero.");
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var result = await bitCoinDataService
            .GetBtcMarketTicksAsync(currencyCodes, page, pageSize)
            .ConfigureAwait(false);

        return Ok(result);
    }

    /// <summary>
    /// Gets a persisted BTC market tick by its CoinDesk sequence number.
    /// </summary>
    [HttpGet("{marketTickId:long:min(1)}")]
    [ProducesResponseType<MarketTick>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MarketTick>> GetBtcMarketTickAsync(
        long marketTickId,
        [FromQuery] CurrencyCodes currencyCodes = DefaultCurrency)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var marketTick = await bitCoinDataService
            .GetBtcMarketTickAsync(marketTickId, currencyCodes)
            .ConfigureAwait(false);

        return marketTick is null ? NotFound() : Ok(marketTick);
    }

    /// <summary>
    /// Fetches the latest BTC market tick and persists it in EUR.
    /// </summary>
    [HttpGet("latest")]
    [ProducesResponseType<MarketTick>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<MarketTick>> GetLatestBtcMarketTickAsync(
        [FromQuery] CurrencyCodes currencyCodes = DefaultCurrency)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var marketTick = await bitCoinDataService
            .FetchLatestBtcMarketTickAsync(currencyCodes: currencyCodes)
            .ConfigureAwait(false);

        return marketTick is null
            ? Problem(
                statusCode: StatusCodes.Status502BadGateway,
                detail: "The latest BTC market tick could not be retrieved.")
            : Ok(marketTick);
    }

    /// <summary>
    /// Updates the user-defined note associated with a persisted market tick.
    /// </summary>
    [HttpPost("{marketTickId:long}/note")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateNoteForMarketTickAsync(
        long marketTickId,
        [FromBody] UpdateMarketTickNoteRequest request)
    {
        if (request.Note?.Length > 1024)
        {
            ModelState.AddModelError(nameof(request.Note), "Note cannot exceed 1024 characters.");
            return ValidationProblem(ModelState);
        }

        var updated = await bitCoinDataService
            .UpdateNoteForMarketTickAsync(marketTickId, request.Note)
            .ConfigureAwait(false);

        return updated ? NoContent() : NotFound();
    }

    /// <summary>
    /// Deletes a persisted BTC market tick by its CoinDesk sequence number.
    /// </summary>
    [HttpDelete("{marketTickId:long:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBtcMarketTickAsync(long marketTickId)
    {
        var deleted = await bitCoinDataService
            .DeleteBtcMarketTickAsync(marketTickId)
            .ConfigureAwait(false);

        return deleted ? NoContent() : NotFound();
    }

    /// <summary>
    /// Deletes persisted BTC market ticks by their CoinDesk sequence numbers.
    /// </summary>
    [HttpDelete]
    [ProducesResponseType<DeleteMarketTicksResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DeleteMarketTicksResponse>> DeleteBtcMarketTicksAsync(
        [FromBody] DeleteMarketTicksRequest request)
    {
        if (request.Ccseqs is null || request.Ccseqs.Count == 0)
        {
            ModelState.AddModelError(nameof(request.Ccseqs), "At least one CCSEQ is required.");
        }
        else if (request.Ccseqs.Any(ccseq => ccseq <= 0))
        {
            ModelState.AddModelError(nameof(request.Ccseqs), "All CCSEQ values must be greater than zero.");
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var deletedCount = await bitCoinDataService
            .DeleteBtcMarketTicksAsync(request.Ccseqs!)
            .ConfigureAwait(false);

        return Ok(new DeleteMarketTicksResponse(deletedCount));
    }
}

public sealed record UpdateMarketTickNoteRequest(string? Note);
public sealed record DeleteMarketTicksRequest(IReadOnlyCollection<long>? Ccseqs);
public sealed record DeleteMarketTicksResponse(int DeletedCount);
