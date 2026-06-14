using BitcoinMarketLoader.Domain.Enums;

namespace BitcoinMarketLoader.Domain.Extensions;

public static class ModelExtensions
{
    extension(TimeWindowRanges range)
    {
        /// <summary>
        /// Gets the start of the time range computed from a range end.
        /// </summary>
        /// <remarks>Week is assumed to start on Sunday</remarks>
        /// <param name="rangeEnd">The end of the time range.</param>
        /// <returns>The start of the time range in the same time zone as input</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public DateTime RangeStart(DateTime rangeEnd)
        {
            return range switch
            {
                TimeWindowRanges.CurrentHour => new DateTime(rangeEnd.Year, rangeEnd.Month, rangeEnd.Day, rangeEnd.Hour, 0, 0, rangeEnd.Kind),
                TimeWindowRanges.CurrentDay => new DateTime(rangeEnd.Year, rangeEnd.Month, rangeEnd.Day, 0, 0, 0,  rangeEnd.Kind),
                TimeWindowRanges.CurrentWeek => new DateTime(rangeEnd.Year, rangeEnd.Month, rangeEnd.Day, 0, 0, 0,  rangeEnd.Kind).AddDays(-(int)rangeEnd.DayOfWeek),
                TimeWindowRanges.CurrentMonth => new DateTime(rangeEnd.Year, rangeEnd.Month, 1, 0, 0, 0,  rangeEnd.Kind),
                TimeWindowRanges.CurrentYear => new DateTime(rangeEnd.Year, 1, 1, 0, 0, 0,  rangeEnd.Kind),
                TimeWindowRanges.Last24Hours => rangeEnd.AddHours(-24),
                TimeWindowRanges.Last7Days => rangeEnd.AddDays(-7),
                TimeWindowRanges.Last30Days => rangeEnd.AddDays(30),
                TimeWindowRanges.Last90Days => rangeEnd.AddDays(90),
                TimeWindowRanges.Last180Days => rangeEnd.AddDays(180),
                TimeWindowRanges.Last365Days => rangeEnd.AddDays(365),
                _ => throw new ArgumentOutOfRangeException(nameof(range), $"Not expected range value: {range}")
            };
        }
    }
}