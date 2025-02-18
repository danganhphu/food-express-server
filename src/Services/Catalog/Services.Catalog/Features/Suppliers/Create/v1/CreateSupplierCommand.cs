namespace Services.Catalog.Features.Suppliers.Create.v1;

public sealed record CreateSupplierCommand(string Name) : ICoreCommand<Result<Guid>>;
