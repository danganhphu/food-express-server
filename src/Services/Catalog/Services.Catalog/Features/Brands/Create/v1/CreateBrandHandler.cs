namespace Services.Catalog.Features.Brands.Create.v1;

public sealed class CreateBrandHandler([FromKeyedServices("catalog")] IEfRepository<Brand> repository)
    : ICoreCommandHandler<CreateBrandCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateBrandCommand command, CancellationToken cancellationToken)
    {
        var brand = await repository.AddAsync(new(command.Name), cancellationToken);

        return Result.Success(brand.Id.Value);
    }
}
