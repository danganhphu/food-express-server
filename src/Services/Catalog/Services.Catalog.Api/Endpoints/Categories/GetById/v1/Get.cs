using Services.Catalog.Features.Categories.GetById.v1;

namespace Services.Catalog.Api.Endpoints.Categories.GetById.v1;

public sealed class GetById(ISender sender) : Endpoint<GetCategoryByIdRequest, CategoryResponse>
{
    public override void Configure()
    {
        Get(ApiRoutes.Category.GetById);

        Group<CategoryGrouping>();
    }

    public override async Task HandleAsync(GetCategoryByIdRequest request,
                                           CancellationToken ct)
    {
        var result = await sender.Send(new GetCategoryByIdQuery(request.Id), ct);

        if (result.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(ct);

            return;
        }

        if (result.IsSuccess)
        {
            Response = new(result.Value.CategoryId.Value, result.Value.Name, result.Value.Code);
        }
    }
}
