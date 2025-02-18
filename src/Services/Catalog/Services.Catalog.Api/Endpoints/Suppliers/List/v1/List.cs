using Services.Catalog.Features.Brands.List.v1;
using Services.Catalog.Features.Suppliers.List.v1;

namespace Services.Catalog.Api.Endpoints.Suppliers.List.v1;

public sealed class List(ISender sender) : EndpointWithoutRequest<SuppliersListResponse>
{
    public override void Configure()
    {
        Get(ApiRoutes.Supplier.List);

        Group<SupplierGrouping>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await sender.Send(new ListSuppliersQuery(), ct);

        if (result.IsSuccess)
        {
            Response = new()
            {
                Suppliers = result.Value.Select(c => new SupplierResponse(c.SupplierId.Value, c.Name)).ToArray()
            };

            return;
        }

        await SendResultAsync(result.ToMinimalApiResult());
    }
}
