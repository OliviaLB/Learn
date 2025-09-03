namespace Insurwave.Movie.ServiceTests.Infrastructure.Extensions;

/// PostgreSQL stores timestamp and timestamptz with microsecond precision, which means PostgreSQL supports up to 6 digits after the decimal for seconds.
/// .NET DateTime supports ticks (100 nanoseconds = 0.0001 ms = 7-digit precision).
public static class DateTimeExtensions
{
    public static DateTime TruncateToMicroseconds(this DateTime sourceDateTime)
    {
        var ticksPerMicrosecond = TimeSpan.TicksPerMillisecond / 1000; // 10
        var truncatedTicks = sourceDateTime.Ticks - (sourceDateTime.Ticks % ticksPerMicrosecond);
        return new DateTime(truncatedTicks, sourceDateTime.Kind);
    }
}
