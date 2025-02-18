using Services.Catalog.Domain.CategoriesAggregate.Specifications;

namespace Services.Catalog.Features.Categories.GetById.v1;

public sealed class GetCategoryByIdHandler(
    [FromKeyedServices("catalog:read")] IEfReadRepository<Category> readRepository)
    : ICoreQueryHandler<GetCategoryByIdQuery, Result<CategoryDto>>
{
    public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var categoryId = Guard.Against.NullOrEmpty(query.CategoryId);

        var category = await readRepository.FirstOrDefaultAsync(new CategoryByIdSpec(categoryId), cancellationToken);

        return category is null
                   ? Result.NotFound($"Category item with id {query.CategoryId} not found")
                   : category.ToCategoryDto();
    }
}
