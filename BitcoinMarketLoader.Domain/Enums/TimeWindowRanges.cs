namespace BitcoinMarketLoader.Domain.Enums;

public enum TimeWindowRanges
{
    CurrentHour = 0,
    CurrentDay = 1,
    CurrentWeek = 2,
    CurrentMonth = 3,
    CurrentYear = 4,
    Last24Hours = 5,
    Last7Days = 6,
    Last30Days = 7,
    Last90Days = 8,
    Last180Days = 9,
    Last365Days = 10,
    LifeTime = 11,
}