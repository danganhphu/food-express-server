using Services.Catalog.Features.Brands.List.v1;

namespace Services.Catalog.Api.Endpoints.Brands.List.v1;

public sealed class List(ISender sender) : EndpointWithoutRequest<BrandListResponse>
{
    public override void Configure()
    {
        Get(ApiRoutes.Brand.List);

        Group<BrandGrouping>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await sender.Send(new ListBrandsQuery(), ct);

        if (result.IsSuccess)
        {
            Response = new()
            {
                Brands = result.Value.Select(c => new BrandResponse(c.BrandId.Value, c.Name)).ToArray()
            };

            return;
        }

        await SendResultAsync(result.ToMinimalApiResult());
    }
}
