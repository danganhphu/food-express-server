namespace Services.Catalog.Features.Brands.CreateBrand.v1;

public sealed record CreateBrandCommand(string Name) : ICoreCommand<Result<Guid>>;
