namespace Services.Catalog.Domain.SupplierAggregate.Specifications;

internal sealed class SupplierByIdSpec : SingleResultSpecification<Supplier>
{
    public SupplierByIdSpec(Guid supplierId)
    {
        Query.Where(supplier => supplier.Id == new SupplierId(supplierId))
             .AsNoTracking();
    }
}
