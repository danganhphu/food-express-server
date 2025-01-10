using System.Collections.Generic;

namespace Services.Catalog.Domain.ProductAggregate.ValueObjects;

public sealed class Name : ValueObject
{
    private Name() { }

    public Name(string? name)
    {
        Value = Guard.Against.NullOrEmpty(name);
    }

    public string? Value { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value ?? string.Empty;
    }
}
