namespace Services.Catalog.Features.Categories.Delete.v1;

public sealed record DeleteCategoryCommand(Guid Id) : ICoreCommand<Result>;
