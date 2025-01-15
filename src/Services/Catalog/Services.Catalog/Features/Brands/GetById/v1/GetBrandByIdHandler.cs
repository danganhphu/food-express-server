using Services.Catalog.Domain.BrandAggregate.Specifications;

namespace Services.Catalog.Features.Brands.GetById.v1;

public sealed class GetBrandByIdHandler([FromKeyedServices("catalog:read")] IEfReadRepository<Brand> readRepository)
    : ICoreQueryHandler<GetBrandByIdQuery, Result<BrandDto>>
{
    public async Task<Result<BrandDto>> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
    {
        var brandId = Guard.Against.NullOrEmpty(request.BrandId);

        var brand = await readRepository.FirstOrDefaultAsync(new BrandByIdSpec(brandId), cancellationToken)
                                        .ConfigureAwait(false);

        return brand is null
                   ? Result<BrandDto>.NotFound($"Brand item with id {request.BrandId} not found")
                   : brand.ToBrandDto();
    }
}
