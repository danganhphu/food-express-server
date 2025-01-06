namespace BuildingBlocks.Core.IdsGenerator;

[DebuggerStepThrough]
public static class GuidIdGenerator
{
    public static Guid NewGuid()
    {
        return Guid.CreateVersion7();
    }

    public static bool BeAGuid(string guidString)
    {
        return Guid.TryParse(guidString, out _);
    }
}
