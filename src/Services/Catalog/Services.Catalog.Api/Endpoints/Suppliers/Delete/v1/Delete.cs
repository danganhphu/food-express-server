using Services.Catalog.Features.Brands.Delete.v1;
using Services.Catalog.Features.Suppliers.Delete.v1;

namespace Services.Catalog.Api.Endpoints.Suppliers.Delete.v1;

public sealed class Delete(ISender sender) : Endpoint<DeleteSupplierRequest>
{
    public override void Configure()
    {
        Delete(ApiRoutes.Supplier.Delete);

        Group<SupplierGrouping>();
    }

    public override async Task HandleAsync(DeleteSupplierRequest request,
                                           CancellationToken ct)
    {
        var result = await sender.Send(new DeleteSupplierCommand(request.Id), ct);

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
