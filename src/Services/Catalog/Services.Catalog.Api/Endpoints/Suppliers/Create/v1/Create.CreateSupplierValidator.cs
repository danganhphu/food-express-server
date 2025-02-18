namespace Services.Catalog.Api.Endpoints.Suppliers.Create.v1;

internal sealed class CreateSupplierValidator : Validator<CreateSupplierRequest>
{
    public CreateSupplierValidator()
    {
        RuleFor(x => x.Name).MaximumLength(50).WithErrorCode("ML001");
    }
}
