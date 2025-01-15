namespace Services.Catalog.Domain.ProductAggregate;

public sealed class ProductStatus : SmartEnum<ProductStatus>
{
    public static readonly ProductStatus Available = new(nameof(Available), 1);
    public static readonly ProductStatus Unavailable = new(nameof(Unavailable), 2);

    internal ProductStatus(string name, int value) : base(name, value) { }
}
