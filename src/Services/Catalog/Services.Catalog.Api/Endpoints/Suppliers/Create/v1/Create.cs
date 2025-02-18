using Services.Catalog.Api.Endpoints.Brands.Create.v1;
using Services.Catalog.Features.Suppliers.Create.v1;

namespace Services.Catalog.Api.Endpoints.Suppliers.Create.v1;

public sealed class Create(ISender sender) : Endpoint<CreateSupplierRequest, CreateBrandResponse>
{
    public override void Configure()
    {
        Post(ApiRoutes.Supplier.Create);

        Group<SupplierGrouping>();
    }

    public override async Task HandleAsync(CreateSupplierRequest request,
                                           CancellationToken ct)
    {
        var result = await sender.Send(new CreateSupplierCommand(request.Name), ct);

        if (result.IsSuccess)
        {
            Response = new(result.Value);

            return;
        }

        await SendResultAsync(result.ToMinimalApiResult());
    }
}
