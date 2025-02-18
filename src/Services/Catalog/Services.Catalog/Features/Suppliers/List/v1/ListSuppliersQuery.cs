namespace Services.Catalog.Features.Suppliers.List.v1;

public sealed record ListSuppliersQuery : ICoreQuery<Result<IReadOnlyCollection<SupplierDto>>>;
