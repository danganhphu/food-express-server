namespace Services.Catalog.Api.Endpoints.Brands.Delete.v1;

internal sealed class DeleteBrandValidator : Validator<DeleteBrandRequest>
{
    public DeleteBrandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("Brand id cannot null!").WithErrorCode("B001");
    }
}
