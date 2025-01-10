using FastEndpoints;
using MediatR;
using Services.Catalog.Api.Contracts;
using Ardalis.Result.AspNetCore;
namespace Services.Catalog.Api.Endpoints.Brands.GetById;

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
        // var result = await sender.Send(new GetBrandQuery(request.Id), ct).ConfigureAwait(false);
        //
        // await SendResultAsync(result.ToMinimalApiResult()).ConfigureAwait(false);
    }
}
