namespace Services.Catalog.Api.Endpoints.Suppliers.Delete.v1;

public sealed class DeleteSupplierRequest
{
    [BindFrom("id")]
    public Guid Id { get; set; }
}
