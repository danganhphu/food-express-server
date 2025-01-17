namespace Services.Catalog.Api.Endpoints.Brands.Delete.v1;

public sealed class DeleteBrandRequest
{
    [BindFrom("id")]
    public Guid Id { get; set; }
}
