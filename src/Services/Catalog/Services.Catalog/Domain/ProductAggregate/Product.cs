using Services.Catalog.Domain.ProductAggregate.Events;

namespace Services.Catalog.Domain.ProductAggregate;

public sealed class Product : EntityBase<Product, ProductId>, IAggregateRoot
{
    private Product()
    {
        // EF Core
    }

    public Product(string? name,
                   string? size,
                   decimal price,
                   decimal priceSale,
                   CategoryId categoryId,
                   BrandId brandId,
                   SupplierId supplierId)
    {
        Name = new(name);
        Size = new(size);
        Price = new(price, priceSale);
        CategoryId = categoryId;
        BrandId = brandId;
        SupplierId = supplierId;
    }

    public static Product Create(string? name,
                                 string? size,
                                 decimal price,
                                 decimal priceSale,
                                 CategoryId categoryId,
                                 BrandId brandId,
                                 SupplierId supplierId)
    {
        var product = new Product(name, size, price, priceSale, categoryId, brandId, supplierId);

        product.RegisterDomainEvent(new ProductCreatedEvent(product));

        return product;
    }

    public ProductStatus Status { get; private set; } = ProductStatus.Available;
    public Name? Name { get; private set; }
    public Size? Size { get; private set; }
    public Price? Price { get; private set; }

    public Category? Category { get; private set; } = null!;
    public CategoryId? CategoryId { get; private set; }

    public Brand? Brand { get; private set; } = null!;
    public BrandId? BrandId { get; private set; }

    public Supplier? Supplier { get; private set; } = null!;
    public SupplierId? SupplierId { get; private set; }

    public void MarkAsUnavailable()
        => Status = ProductStatus.Unavailable;
}
