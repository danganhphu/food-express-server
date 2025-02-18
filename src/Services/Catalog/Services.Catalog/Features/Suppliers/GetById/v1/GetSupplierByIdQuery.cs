namespace Services.Catalog.Features.Suppliers.GetById.v1;

public sealed record GetSupplierByIdQuery(Guid SupplierId) : ICoreQuery<Result<SupplierDto>>;
