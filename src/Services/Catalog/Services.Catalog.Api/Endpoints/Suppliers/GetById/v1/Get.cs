using Services.Catalog.Features.Suppliers.GetById.v1;

namespace Services.Catalog.Api.Endpoints.Suppliers.GetById.v1;

public sealed class GetById(ISender sender) : Endpoint<GetSupplierByIdRequest, SupplierResponse>
{
    public override void Configure()
    {
        Get(ApiRoutes.Supplier.GetById);

        Group<SupplierGrouping>();
    }

    public override async Task HandleAsync(GetSupplierByIdRequest request,
                                           CancellationToken ct)
    {
        var result = await sender.Send(new GetSupplierByIdQuery(request.Id), ct);

        if (result.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(ct);

            return;
        }

        if (result.IsSuccess)
        {
            Response = new(result.Value.SupplierId.Value, result.Value.Name);
        }
    }
}
