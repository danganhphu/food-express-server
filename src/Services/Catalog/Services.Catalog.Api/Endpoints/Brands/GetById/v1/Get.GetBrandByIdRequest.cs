namespace Services.Catalog.Api.Endpoints.Brands.GetById.v1;

public sealed class GetBrandByIdRequest
{
    [BindFrom("id")]
    public Guid Id { get; set; }
}
