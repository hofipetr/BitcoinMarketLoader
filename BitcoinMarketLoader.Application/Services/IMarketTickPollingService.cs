namespace BitcoinMarketLoader.Application.Services;

public interface IMarketTickPollingService
{
    int PollingIntervalSeconds { get; }

    Task StartPollingAsync(CancellationToken cancellationToken);
}
