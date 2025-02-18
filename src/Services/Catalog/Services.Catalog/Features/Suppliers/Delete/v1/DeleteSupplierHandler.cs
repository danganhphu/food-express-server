using Services.Catalog.Domain.SupplierAggregate.Specifications;

namespace Services.Catalog.Features.Suppliers.Delete.v1;

public sealed class DeleteSupplierHandler([FromKeyedServices("catalog")] IEfRepository<Supplier> repository)
    : ICoreCommandHandler<DeleteSupplierCommand, Result>
{
    public async Task<Result> Handle(DeleteSupplierCommand command, CancellationToken cancellationToken)
    {
        var id = Guard.Against.NullOrEmpty(command.Id);

        var supplier = await repository.GetByIdAsync(new SupplierByIdSpec(id), cancellationToken);

        if (supplier is null)
        {
            return Result.NotFound($"Supplier item with id {id} not found");
        }
        await repository.DeleteAsync(supplier, cancellationToken);

        return Result.Success();
    }
}
