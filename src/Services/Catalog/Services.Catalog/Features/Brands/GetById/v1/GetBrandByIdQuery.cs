namespace Services.Catalog.Features.Brands.GetById.v1;

public sealed record GetBrandByIdQuery(Guid BrandId) : ICoreQuery<Result<BrandDto>>;
