namespace Services.Catalog.Features.Brands.GetById.v1;

public sealed record GetBrandById(Guid BrandId) : IQuery<Result<BrandDto>>;
