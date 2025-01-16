namespace Services.Catalog.Api.Endpoints.Brands.GetById.v1;

public sealed class GetById(ISender sender) : Endpoint<GetBrandByIdRequest, BrandResponse>
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

        if (result.IsSuccess)
        {
            Response = new(result.Value.BrandId.Value, result.Value.Name);

            return;
        }

        await SendResultAsync(result.ToMinimalApiResult());
    }
}
