namespace Services.Catalog.Features.Suppliers.Delete.v1;

public sealed record DeleteSupplierCommand(Guid Id) : ICoreCommand<Result>;
