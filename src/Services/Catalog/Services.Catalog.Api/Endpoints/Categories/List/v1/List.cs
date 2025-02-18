using Services.Catalog.Features.Brands.List.v1;
using Services.Catalog.Features.Categories.List.v1;

namespace Services.Catalog.Api.Endpoints.Categories.List.v1;

public sealed class List(ISender sender) : EndpointWithoutRequest<CategoriesListResponse>
{
    public override void Configure()
    {
        Get(ApiRoutes.Category.List);

        Group<CategoryGrouping>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await sender.Send(new ListCategoriesQuery(), ct);

        if (result.IsSuccess)
        {
            Response = new()
            {
                Categories = result.Value.Select(c => new CategoryResponse(c.CategoryId.Value, c.Name, c.Code)).ToArray()
            };

            return;
        }

        await SendResultAsync(result.ToMinimalApiResult());
    }
}
