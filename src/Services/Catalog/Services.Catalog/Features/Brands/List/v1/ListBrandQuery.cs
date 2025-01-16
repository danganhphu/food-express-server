namespace Services.Catalog.Features.Brands.List.v1;

public sealed record ListBrandQuery : ICoreQuery<Result<IReadOnlyCollection<BrandDto>>>;
