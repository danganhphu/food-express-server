namespace Services.Catalog.Api.Endpoints.Categories;

public sealed class CategoryGrouping : Group
{
    public CategoryGrouping()
    {
        Configure(
            "v{version:apiVersion}/category",
            ep =>
            {
                ep.Description(
                    x => x.Produces(401)
                          .WithTags(nameof(CategoryGrouping))
                          .WithVersionSet(">>Categories<<")
                          .MapToApiVersion(1.0));
            });
    }
}
