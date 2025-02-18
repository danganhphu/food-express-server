namespace Services.Catalog.Api.Endpoints.Categories.GetById.v1;

public sealed class GetCategoryByIdRequest
{
    [BindFrom("id")]
    public Guid Id { get; set; }
}
