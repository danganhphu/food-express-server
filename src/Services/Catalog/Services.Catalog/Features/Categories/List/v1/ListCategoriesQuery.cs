using Services.Catalog.Features.Brands;

namespace Services.Catalog.Features.Categories.List.v1;

public sealed record ListCategoriesQuery : ICoreQuery<Result<IReadOnlyCollection<CategoryDto>>>;
