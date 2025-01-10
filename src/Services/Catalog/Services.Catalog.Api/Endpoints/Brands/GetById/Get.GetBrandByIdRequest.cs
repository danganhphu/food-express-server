using FastEndpoints;

namespace Services.Catalog.Api.Endpoints.Brands.GetById;

public sealed class GetBrandByIdRequest
{
    [BindFrom("id")]
    public Guid Id { get; set; }
}
