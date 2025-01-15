namespace Services.Catalog.Features.Brands.Create.v1;

public sealed record CreateBrandCommand(string Name) : ICoreCommand<Result<Guid>>;
