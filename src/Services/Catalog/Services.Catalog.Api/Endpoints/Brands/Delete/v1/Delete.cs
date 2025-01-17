using Services.Catalog.Features.Brands.Delete.v1;

namespace Services.Catalog.Api.Endpoints.Brands.Delete.v1;

public sealed class Delete(ISender sender) : Endpoint<DeleteBrandRequest>
{
    public override void Configure()
    {
        Delete(ApiRoutes.Brand.Delete);

        Group<BrandGrouping>();
    }

    public override async Task HandleAsync(DeleteBrandRequest request,
                                           CancellationToken ct)
    {
        var result = await sender.Send(new DeleteBrandCommand(request.Id), ct);

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
