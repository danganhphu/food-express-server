namespace Services.Catalog.Features.Categories.Create.v1;

public sealed record CreateCategoryCommand(string Name, string Code) : ICoreCommand<Result<Guid>>;
