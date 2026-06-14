using BitcoinMarketLoader.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BitcoinMarketLoader.Api.Controllers;

[ApiController]
[Route("polling")]
public class PollingController(IMarketTickPollingService pollingService) : ControllerBase
{
    /// <summary>
    /// Gets the market tick polling interval in seconds, or zero when polling is disabled.
    /// </summary>
    [HttpGet("interval")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public ActionResult<int> GetPollingInterval()
    {
        return Ok(pollingService.PollingIntervalSeconds);
    }
}
