namespace Services.Catalog.Features.Categories;

internal static class CategoryMappings
{
    internal static CategoryDto ToCategoryDto(this Category category)
        => new(category.Id.Value, category.Name, category.Code);

    internal static IReadOnlyCollection<CategoryDto> ToCategoryDtos(this IEnumerable<Category> categories)
        => categories.Select(ToCategoryDto).ToArray();
}
