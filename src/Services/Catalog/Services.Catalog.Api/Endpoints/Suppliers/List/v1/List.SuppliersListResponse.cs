namespace Services.Catalog.Api.Endpoints.Suppliers.List.v1;

public sealed class SuppliersListResponse
{
    public IReadOnlyCollection<SupplierResponse> Suppliers { get; set; } = [];
}
