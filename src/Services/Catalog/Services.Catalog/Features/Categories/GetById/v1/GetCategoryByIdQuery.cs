using Services.Catalog.Features.Brands;

namespace Services.Catalog.Features.Categories.GetById.v1;

public sealed record GetCategoryByIdQuery(Guid CategoryId) : ICoreQuery<Result<CategoryDto>>;
