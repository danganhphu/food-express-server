namespace Services.Catalog.Domain.BrandAggregate.Specifications;

internal sealed class BrandByIdSpec : SingleResultSpecification<Brand>
{
    public BrandByIdSpec(Guid brandId)
    {
        Query.Where(brand => brand.Id == new BrandId(brandId))
             .AsNoTracking();
    }
}
