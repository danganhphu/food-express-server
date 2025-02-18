namespace Services.Catalog.Api.Endpoints.Categories.Delete.v1;

internal sealed class DeleteCategoryValidator : Validator<DeleteCategoryRequest>
{
    public DeleteCategoryValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("Category id cannot null!").WithErrorCode("Ca001");
    }
}
