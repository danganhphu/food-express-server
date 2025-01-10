using Ardalis.Result;
using BuildingBlocks.Core.EFCore;
using Services.Catalog.Domain.BrandAggregate.Specifications;

namespace Services.Catalog.Features.Brands.GetBrandById.v1;

internal sealed class GetBrandByIdHandler([FromKeyedServices("catalog:read")] IEfReadRepository<Brand> readRepository)
    : IQueryHandler<GetBrandById, BrandDto>
{
    public async Task<BrandDto> Handle(GetBrandById request, CancellationToken cancellationToken)
    {
        var brandId = Guard.Against.NullOrEmpty(request.BrandId);

        var brand = await readRepository.FirstOrDefaultAsync(new BrandByIdSpec(brandId), cancellationToken)
                                        .ConfigureAwait(false);

        return brand.ToBrandDto();
    }
}
