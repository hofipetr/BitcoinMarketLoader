using BitcoinMarketLoader.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BitcoinMarketLoader.Api.Controllers;

[ApiController]
[Route("exchangeRates")]
public class ExchangeRatesController(ICzkExchangeRateService exchangeRateService) : ControllerBase
{
    /// <summary>
    /// Gets the CZK exchange rate for one unit of the requested currency.
    /// </summary>
    [HttpGet("{targetCurrency}")]
    [ProducesResponseType<decimal>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<decimal>> GetExchangeRateAsync(
        string targetCurrency,
        [FromQuery] DateTime? date = null)
    {
        if (string.IsNullOrWhiteSpace(targetCurrency))
        {
            ModelState.AddModelError(nameof(targetCurrency), "Target currency is required.");
            return ValidationProblem(ModelState);
        }

        var exchangeRate = await exchangeRateService
            .GetExchangeRate(targetCurrency, date)
            .ConfigureAwait(false);

        return Ok(exchangeRate);
    }
}
