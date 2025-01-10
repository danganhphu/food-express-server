namespace Services.Catalog.Domain.CategoriesAggregate;

public sealed class Category : EntityBase<Category, CategoryId>, IAggregateRoot
{
    private Category()
    {
        // EF Core
    }

    public Category(string? name, string? code)
    {
        Name = Guard.Against.NullOrEmpty(name);
        Code = Guard.Against.NullOrEmpty(code);
    }

    public string? Name { get; private set; }
    public string? Code { get; private set; }
}
