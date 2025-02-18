namespace Services.Catalog.Domain.CategoriesAggregate.Specifications;

internal sealed class CategoryByIdSpec : SingleResultSpecification<Category>
{
    public CategoryByIdSpec(Guid categoryId)
    {
        Query.Where(category => category.Id == new CategoryId(categoryId))
             .AsNoTracking();
    }
}
