namespace Services.Catalog.Features.Brands.List.v1;

public sealed class ListBrandHandler([FromKeyedServices("catalog:read")] IEfReadRepository<Brand> readRepository)
    : ICoreQueryHandler<ListBrandQuery, Result<IReadOnlyCollection<BrandDto>>>
{
    public async Task<Result<IReadOnlyCollection<BrandDto>>> Handle(ListBrandQuery query,
                                                                    CancellationToken cancellationToken)
    {
        Guard.Against.Null(query);

        var brands = await readRepository.ListAsync(cancellationToken);

        return Result.Success(brands.ToBrandDtos());
    }
}
