namespace Services.Catalog.Api.Endpoints.Brands.GetById.v1;

internal sealed class GetBrandByIdRequest
{
    [BindFrom("id")]
    public Guid Id { get; set; }
}
