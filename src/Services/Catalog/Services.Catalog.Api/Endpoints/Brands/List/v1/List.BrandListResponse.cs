namespace Services.Catalog.Api.Endpoints.Brands.List.v1;

public sealed class BrandListResponse
{
    public List<BrandResponse> Brands { get; set; } = [];
}
