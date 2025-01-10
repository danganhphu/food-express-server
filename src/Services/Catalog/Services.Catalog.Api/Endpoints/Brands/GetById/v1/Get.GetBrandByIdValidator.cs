namespace Services.Catalog.Api.Endpoints.Brands.GetById.v1;

internal sealed class GetBrandByIdValidator : Validator<GetBrandByIdRequest>
{
    internal GetBrandByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("Brand id cannot null!").WithErrorCode("B001");
    }
}
