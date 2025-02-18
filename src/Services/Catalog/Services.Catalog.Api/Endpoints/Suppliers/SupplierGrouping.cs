namespace Services.Catalog.Api.Endpoints.Suppliers;

public sealed class SupplierGrouping : Group
{
    public SupplierGrouping()
    {
        Configure(
            "v{version:apiVersion}/supplier",
            ep =>
            {
                ep.Description(
                    x => x.Produces(401)
                          .WithTags(nameof(SupplierGrouping))
                          .WithVersionSet(">>Suppliers<<")
                          .MapToApiVersion(1.0));
            });
    }
}
