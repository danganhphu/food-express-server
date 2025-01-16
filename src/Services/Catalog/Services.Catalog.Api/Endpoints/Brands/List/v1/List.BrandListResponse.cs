namespace Services.Catalog.Api.Endpoints.Brands.List.v1;

public sealed class BrandListResponse
{
    public IReadOnlyCollection<BrandResponse> Brands { get; set; } = [];
}
