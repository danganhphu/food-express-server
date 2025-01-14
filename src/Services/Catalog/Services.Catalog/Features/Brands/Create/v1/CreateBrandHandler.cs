using BuildingBlocks.Core.EFCore;

namespace Services.Catalog.Features.Brands.CreateBrand.v1;

internal sealed class CreateBrandHandler([FromKeyedServices("catalog")] IEfRepository<Brand> readRepository)
    : ICoreCommandHandler<CreateBrandCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateBrandCommand command, CancellationToken cancellationToken)
    {
        var brand = await readRepository.AddAsync(new(command.Name), cancellationToken);

        return Result.Success(brand.Id.Value);
    }
}
