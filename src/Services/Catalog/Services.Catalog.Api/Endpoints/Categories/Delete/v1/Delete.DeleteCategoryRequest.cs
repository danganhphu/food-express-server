namespace Services.Catalog.Api.Endpoints.Categories.Delete.v1;

public sealed class DeleteCategoryRequest
{
    [BindFrom("id")]
    public Guid Id { get; set; }
}
