using Ardalis.Result;

namespace Services.Catalog.Features.Brands.GetBrandById.v1;

public sealed record GetBrandById(Guid BrandId) : IQuery<BrandDto>;
