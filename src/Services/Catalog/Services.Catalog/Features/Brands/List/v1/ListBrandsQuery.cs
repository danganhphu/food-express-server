namespace Services.Catalog.Features.Brands.List.v1;

public sealed record ListBrandsQuery : ICoreQuery<Result<IReadOnlyCollection<BrandDto>>>;
