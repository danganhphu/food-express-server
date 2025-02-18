namespace Services.Catalog.Api.Endpoints.Categories;

public sealed record CategoryResponse(Guid? CategoryId, string? Name, string? Code);
