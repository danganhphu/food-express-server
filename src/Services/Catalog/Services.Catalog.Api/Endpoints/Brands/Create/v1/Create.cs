namespace Services.Catalog.Api.Endpoints.Brands.Create.v1;

public sealed class Create(ISender sender) : Endpoint<CreateBrandRequest, CreateBrandResponse>
{
    public override void Configure()
    {
        Post(ApiRoutes.Brand.Create);

        Group<BrandGrouping>();
    }

    public override async Task HandleAsync(CreateBrandRequest request,
                                           CancellationToken ct)
    {
        var result = await sender.Send(new CreateBrandCommand(request.Name), ct);

        if (result.IsSuccess)
        {
            Response = new(result.Value);

            return;
        }

        await SendResultAsync(result.ToMinimalApiResult());
    }
}
