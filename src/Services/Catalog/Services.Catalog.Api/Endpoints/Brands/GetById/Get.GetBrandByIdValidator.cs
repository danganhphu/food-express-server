using FastEndpoints;
using FluentValidation;

namespace Services.Catalog.Api.Endpoints.Brands.GetById;

internal sealed class GetBrandByIdValidator : Validator<GetBrandByIdRequest>
{
    public GetBrandByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("Brand id cannot null!").WithErrorCode("B001");
    }
}
