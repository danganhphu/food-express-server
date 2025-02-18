namespace Services.Catalog.Features.Categories.List.v1;

public sealed class ListCategoriesHandler(
    [FromKeyedServices("catalog:read")] IEfReadRepository<Category> readRepository)
    : ICoreQueryHandler<ListCategoriesQuery, Result<IReadOnlyCollection<CategoryDto>>>
{
    public async Task<Result<IReadOnlyCollection<CategoryDto>>> Handle(ListCategoriesQuery query,
                                                                       CancellationToken cancellationToken)
    {
        Guard.Against.Null(query);

        var categories = await readRepository.ListAsync(cancellationToken);

        return categories.Count == 0
                   ? Result.NotFound("No categories found.")
                   : Result.Success(categories.ToCategoryDtos());
    }
}
