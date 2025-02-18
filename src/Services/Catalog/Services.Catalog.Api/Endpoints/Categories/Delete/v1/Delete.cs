using Services.Catalog.Features.Brands.Delete.v1;
using Services.Catalog.Features.Categories.Delete.v1;

namespace Services.Catalog.Api.Endpoints.Categories.Delete.v1;

public sealed class Delete(ISender sender) : Endpoint<DeleteCategoryRequest>
{
    public override void Configure()
    {
        Delete(ApiRoutes.Category.Delete);

        Group<CategoryGrouping>();
    }

    public override async Task HandleAsync(DeleteCategoryRequest request,
                                           CancellationToken ct)
    {
        var result = await sender.Send(new DeleteCategoryCommand(request.Id), ct);

        if (result.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(ct);

            return;
        }

        if (result.IsSuccess)
        {
            await SendNoContentAsync(ct);
        }
    }
}
