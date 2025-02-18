namespace Services.Catalog.Features.Brands.List.v1;

public sealed class ListBrandsHandler([FromKeyedServices("catalog:read")] IEfReadRepository<Brand> readRepository)
    : ICoreQueryHandler<ListBrandsQuery, Result<IReadOnlyCollection<BrandDto>>>
{
    public async Task<Result<IReadOnlyCollection<BrandDto>>> Handle(ListBrandsQuery query,
                                                                    CancellationToken cancellationToken)
    {
        Guard.Against.Null(query);

        var brands = await readRepository.ListAsync(cancellationToken);

        return brands.Count == 0
                   ? Result.NotFound("No brands found.")
                   : Result.Success(brands.ToBrandDtos());
    }
}
