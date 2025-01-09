using Ardalis.GuardClauses;
using BuildingBlocks.Core.Domain;
using BuildingBlocks.Core.Domain.Abstractions;

namespace Services.Catalog.Domain.BrandAggregate;

public sealed class Brand : EntityBase<Brand, BrandId>, IAggregateRoot
{
    private Brand()
    {
        // EF Core
    }

    public Brand(string? name)
    {
        Name = Guard.Against.NullOrEmpty(name);
    }

    public string? Name { get; private set; }
}
