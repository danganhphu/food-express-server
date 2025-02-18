namespace Services.Catalog.Features.Suppliers.List.v1;

public sealed class ListSuppliersHandler([FromKeyedServices("catalog:read")] IEfReadRepository<Supplier> readRepository)
    : ICoreQueryHandler<ListSuppliersQuery, Result<IReadOnlyCollection<SupplierDto>>>
{
    public async Task<Result<IReadOnlyCollection<SupplierDto>>> Handle(ListSuppliersQuery query,
                                                                       CancellationToken cancellationToken)
    {
        Guard.Against.Null(query);

        var suppliers = await readRepository.ListAsync(cancellationToken);

        return suppliers.Count == 0
                   ? Result.NotFound("No suppliers found.")
                   : Result.Success(suppliers.ToSupplierDtos());
    }
}
