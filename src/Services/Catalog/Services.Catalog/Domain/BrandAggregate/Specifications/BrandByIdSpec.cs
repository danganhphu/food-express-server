namespace Services.Catalog.Domain.BrandAggregate.Specifications;

internal sealed class BrandByIdSpec : Specification<Brand>
{
    public BrandByIdSpec(Guid brandId)
    {
        Query.Where(brand => brand.Id == new BrandId(brandId))
             .AsNoTracking();
    }
}
