namespace BuildingBlocks.Core.Clock;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;

    public DateTime UtcNow => DateTime.UtcNow;

    public DateTimeOffset OffsetNow => DateTimeOffset.Now;

    public DateTimeOffset OffsetUtcNow => DateTimeOffset.UtcNow;
}
