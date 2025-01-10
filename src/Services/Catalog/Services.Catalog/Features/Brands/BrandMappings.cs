namespace Services.Catalog.Features.Brands;

internal static class BrandMappings
{
    internal static BrandDto ToBrandDto(this Brand brand)
        => new BrandDto(brand.Id.Value, brand.Name);
}
