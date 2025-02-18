using Services.Catalog.Domain.SupplierAggregate.Specifications;

namespace Services.Catalog.Features.Suppliers.GetById.v1;

public sealed class GetSupplierByIdHandler(
    [FromKeyedServices("catalog:read")] IEfReadRepository<Supplier> readRepository)
    : ICoreQueryHandler<GetSupplierByIdQuery, Result<SupplierDto>>
{
    public async Task<Result<SupplierDto>> Handle(GetSupplierByIdQuery query, CancellationToken cancellationToken)
    {
        var supplierId = Guard.Against.NullOrEmpty(query.SupplierId);

        var supplier = await readRepository.FirstOrDefaultAsync(new SupplierByIdSpec(supplierId), cancellationToken);

        return supplier is null
                   ? Result.NotFound($"Supplier item with id {query.SupplierId} not found")
                   : supplier.ToSupplierDto();
    }
}
