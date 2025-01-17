namespace Services.Catalog.Features.Brands.Delete.v1;

public sealed record DeleteBrandCommand(Guid Id) : ICoreCommand<Result>;
