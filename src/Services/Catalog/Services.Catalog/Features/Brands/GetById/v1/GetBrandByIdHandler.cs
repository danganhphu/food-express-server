using Microsoft.Extensions.Caching.Hybrid;
using Services.Catalog.Domain.BrandAggregate.Specifications;

namespace Services.Catalog.Features.Brands.GetById.v1;

public sealed class GetBrandByIdHandler([FromKeyedServices("catalog:read")] IEfReadRepository<Brand> readRepository,
                                        HybridCache cache)
    : ICoreQueryHandler<GetBrandByIdQuery, Result<BrandDto>>
{
    public async Task<Result<BrandDto>> Handle(GetBrandByIdQuery query, CancellationToken cancellationToken)
    {
        var brandId = Guard.Against.NullOrEmpty(query.BrandId);

        var brand = await readRepository.FirstOrDefaultAsync(new BrandByIdSpec(brandId), cancellationToken);

        return brand is null
                   ? Result.NotFound($"Brand item with id {query.BrandId} not found")
                   : brand.ToBrandDto();
    }
}
