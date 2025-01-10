namespace Services.Catalog.Api.Endpoints.Brands;

public sealed class BrandGrouping : Group
{
    public BrandGrouping()
    {
        Configure(
            "{v:apiVersion}/brand",
            ep =>
            {
                ep.Description(
                    x => x.Produces(401)
                          .WithTags(nameof(BrandGrouping))
                          .WithVersionSet(">>Brands<<")
                          .MapToApiVersion(1.0));
            });
    }
}
