namespace Services.Catalog.Features.Brands.Delete.v1;

public sealed class DeleteBrandHandler([FromKeyedServices("catalog")] IEfRepository<Brand> repository)
    : ICoreCommandHandler<DeleteBrandCommand, Result>
{
    public async Task<Result> Handle(DeleteBrandCommand command, CancellationToken cancellationToken)
    {
        var id = Guard.Against.NullOrEmpty(command.Id);

        var brand = await repository.GetByIdAsync(new BrandByIdSpec(id), cancellationToken);

        if (brand is null)
        {
            return Result.NotFound($"Brand item with id {id} not found");
        } 
        await repository.DeleteAsync(brand, cancellationToken);

        return Result.Success();
    }
}
