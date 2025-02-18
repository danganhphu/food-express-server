namespace Services.Catalog.Features.Categories.Create.v1;

public sealed class CreateCategoryHandler([FromKeyedServices("catalog")] IEfRepository<Category> repository)
    : ICoreCommandHandler<CreateCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await repository.AddAsync(new(command.Name, command.Code), cancellationToken);

        return Result.Success(category.Id.Value);
    }
}
