using Services.Catalog.Domain.CategoriesAggregate.Specifications;

namespace Services.Catalog.Features.Categories.Delete.v1;

public sealed class DeleteCategoryHandler([FromKeyedServices("catalog")] IEfRepository<Category> repository)
    : ICoreCommandHandler<DeleteCategoryCommand, Result>
{
    public async Task<Result> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var id = Guard.Against.NullOrEmpty(command.Id);

        var category = await repository.GetByIdAsync(new CategoryByIdSpec(id), cancellationToken);

        if (category is null)
        {
            return Result.NotFound($"Category item with id {id} not found");
        }
        await repository.DeleteAsync(category, cancellationToken);

        return Result.Success();
    }
}
