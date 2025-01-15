namespace Services.Catalog.Api.Endpoints.Brands.GetById.v1;

public sealed class GetById(ISender sender) : Endpoint<GetBrandByIdRequest, GetBrandByIdResponse>
{
    public override void Configure()
    {
        Get(ApiRoutes.Brand.GetById);

        Group<BrandGrouping>();
    }

    public override async Task HandleAsync(GetBrandByIdRequest request,
                                           CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var result = await sender.Send(new GetBrandByIdQuery(request.Id), ct);

        await SendResultAsync(result.ToMinimalApiResult());

        // if (result.Status == ResultStatus.NotFound)
        // {
        //     await SendNotFoundAsync(ct);
        //
        //     return;
        // }
        //
        // if (result.IsSuccess)
        // {
        //     await SendAsync(result, cancellation: ct);
        // }
    }
}
