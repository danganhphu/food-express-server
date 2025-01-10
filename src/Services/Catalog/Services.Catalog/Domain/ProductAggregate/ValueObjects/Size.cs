namespace Services.Catalog.Domain.ProductAggregate.ValueObjects;

public sealed class Size : ValueObject
{
    private Size() { }

    public Size(string? name)
    {
        Value = Guard.Against.NullOrEmpty(name);
    }

    public string? Value { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value ?? string.Empty;
    }
}
