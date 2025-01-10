namespace Services.Catalog.Domain.SupplierAggregate;

public sealed class Supplier : EntityBase<Supplier, SupplierId>, IAggregateRoot
{
    private Supplier() { }

    public Supplier(string? name)
    {
        Name = Guard.Against.NullOrEmpty(name);
    }

    public string? Name { get; private set; }
}
