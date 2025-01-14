using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Services.Catalog.Features.Brands.GetById.v1;

namespace Services.Catalog.Api.Endpoints.Brands.GetById.v1;

public sealed class GetById(ISender sender) : Endpoint<GetBrandByIdRequest>
{
    public override void Configure()
    {
        Get(ApiRoutes.Brand.GetById);
        AllowAnonymous();

        Group<BrandGrouping>();
    }

    public override async Task HandleAsync(GetBrandByIdRequest request,
                                           CancellationToken ct)
    {
        var result = await sender.Send(new GetBrandById(request.Id), ct);

        // stuff: https://github.com/ardalis/Result/issues/225
        if (result.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(ct);

            return;
        }

        if (result.IsSuccess)
        {
            await SendAsync(result, cancellation: ct);
        }
    }
}
