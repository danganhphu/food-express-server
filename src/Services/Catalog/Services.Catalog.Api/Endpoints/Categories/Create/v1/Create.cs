using Services.Catalog.Features.Categories.Create.v1;

namespace Services.Catalog.Api.Endpoints.Categories.Create.v1;

public sealed class Create(ISender sender) : Endpoint<CreateCategoryRequest, CreateCategoryResponse>
{
    public override void Configure()
    {
        Post(ApiRoutes.Category.Create);

        Group<CategoryGrouping>();
    }

    public override async Task HandleAsync(CreateCategoryRequest request,
                                           CancellationToken ct)
    {
        var result = await sender.Send(new CreateCategoryCommand(request.Name, request.Code), ct);

        if (result.IsSuccess)
        {
            Response = new(result.Value);

            return;
        }

        await SendResultAsync(result.ToMinimalApiResult());
    }
}
