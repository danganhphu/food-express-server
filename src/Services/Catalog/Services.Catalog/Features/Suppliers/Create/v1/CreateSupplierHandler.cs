namespace Services.Catalog.Features.Suppliers.Create.v1;

public sealed class CreateSupplierHandler([FromKeyedServices("catalog")] IEfRepository<Supplier> repository)
    : ICoreCommandHandler<CreateSupplierCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateSupplierCommand command, CancellationToken cancellationToken)
    {
        var supplier = await repository.AddAsync(new(command.Name), cancellationToken);

        return Result.Success(supplier.Id.Value);
    }
}
