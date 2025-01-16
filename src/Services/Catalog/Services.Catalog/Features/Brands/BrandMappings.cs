namespace Services.Catalog.Features.Brands;

internal static class BrandMappings
{
    internal static BrandDto ToBrandDto(this Brand brand)
        => new(brand.Id.Value, brand.Name);

    internal static IReadOnlyCollection<BrandDto> ToBrandDtos(this IEnumerable<Brand> brands)
        => brands.Select(ToBrandDto).ToArray();
}
