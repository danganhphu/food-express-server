namespace Services.Catalog.Api.Endpoints.Brands.List.v1;

public sealed class List(ISender sender) : EndpointWithoutRequest<BrandListResponse>
{
    public override void Configure() { }

    public override async Task HandleAsync(CancellationToken ct) { }
}
