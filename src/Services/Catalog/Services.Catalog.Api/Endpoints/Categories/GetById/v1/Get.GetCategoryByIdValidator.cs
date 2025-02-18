namespace Services.Catalog.Api.Endpoints.Categories.GetById.v1;

internal sealed class GetCategoryByIdValidator : Validator<GetCategoryByIdRequest>
{
    public GetCategoryByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("Category id cannot null!").WithErrorCode("Ca001");
    }
}
