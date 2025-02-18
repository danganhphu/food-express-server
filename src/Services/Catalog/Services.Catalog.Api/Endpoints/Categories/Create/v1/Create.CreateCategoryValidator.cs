namespace Services.Catalog.Api.Endpoints.Categories.Create.v1;

internal sealed class CreateCategoryValidator : Validator<CreateCategoryRequest>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name).MaximumLength(50).WithErrorCode("ML001");
        RuleFor(x => x.Code).MaximumLength(10).WithErrorCode("ML001");
    }
}
