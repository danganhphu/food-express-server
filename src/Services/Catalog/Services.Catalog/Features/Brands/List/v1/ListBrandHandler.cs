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

        var cachedBrands = await cache.GetOrCreateAsync(
                               "brands-list",
                               async token =>
                               {
                                   var brands = await readRepository.ListAsync(token);

                                   return brands;
                               },
                               tags: ["brands"],
                               cancellationToken: cancellationToken);

        return cachedBrands.Count == 0
                   ? Result.NotFound("No brands found.")
                   : Result.Success(cachedBrands.ToBrandDtos());
    }
}
