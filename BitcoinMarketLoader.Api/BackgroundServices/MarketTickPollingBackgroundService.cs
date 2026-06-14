using BitcoinMarketLoader.Application.Services;

namespace BitcoinMarketLoader.Api.BackgroundServices;

public sealed class MarketTickPollingBackgroundService(
    IMarketTickPollingService pollingService) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return pollingService.StartPollingAsync(stoppingToken);
    }
}
