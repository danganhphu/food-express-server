namespace Services.Catalog.Api.Endpoints.Categories.List.v1;

public sealed class CategoriesListResponse
{
    public IReadOnlyCollection<CategoryResponse> Categories { get; set; } = [];
}
