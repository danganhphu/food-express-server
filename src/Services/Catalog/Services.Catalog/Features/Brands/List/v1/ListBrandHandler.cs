using Microsoft.Extensions.Caching.Hybrid;

namespace Services.Catalog.Features.Brands.List.v1;

public sealed class ListBrandHandler([FromKeyedServices("catalog:read")] IEfReadRepository<Brand> readRepository,
                                     HybridCache cache)
    : ICoreQueryHandler<ListBrandQuery, Result<IReadOnlyCollection<BrandDto>>>
{
    public async Task<Result<IReadOnlyCollection<BrandDto>>> Handle(ListBrandQuery query,
                                                                    CancellationToken cancellationToken)
    {
        Guard.Against.Null(query);

        var brands = await readRepository.ListAsync(cancellationToken);

        return brands.Count == 0
                   ? Result.NotFound("No brands found.")
                   : Result.Success(brands.ToBrandDtos());
    }
}
