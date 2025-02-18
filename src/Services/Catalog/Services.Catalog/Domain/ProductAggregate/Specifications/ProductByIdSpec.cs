namespace Services.Catalog.Domain.ProductAggregate.Specifications;

internal sealed class ProductByIdSpec : SingleResultSpecification<Product>
{
    public ProductByIdSpec(Guid productId)
    {
        Query.Where(product => product.Id == new ProductId(productId))
             .AsNoTracking();
    }
}
