namespace Services.Catalog.Api.Endpoints.Suppliers.Delete.v1;

internal sealed class DeleteSupplierValidator : Validator<DeleteSupplierRequest>
{
    public DeleteSupplierValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("Supplier id cannot null!").WithErrorCode("S001");
    }
}
