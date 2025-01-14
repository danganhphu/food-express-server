namespace Services.Catalog.Api.Endpoints.Brands.Create.v1;

internal sealed class CreateBrandValidator : Validator<CreateBrandRequest>
{
    public CreateBrandValidator()
    {
        RuleFor(x => x.Name).MaximumLength(50).WithErrorCode("ML001");
    }
}
