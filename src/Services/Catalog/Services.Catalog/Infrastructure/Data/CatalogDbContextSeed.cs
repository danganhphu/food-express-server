using Services.Catalog.Domain.ProductAggregate;

namespace Services.Catalog.Infrastructure.Data;

public sealed class CatalogDbContextSeed : IDbSeeder<CatalogDbContext>
{
    public async Task SeedAsync(CatalogDbContext context)
    {
        // Seed Categories if they don't exist
        if (!context.Categories.Any())
        {
            await context.Categories.AddRangeAsync(GetPreconfiguredCategories());
            await context.SaveChangesAsync();
        }

        // Seed Brands if they don't exist
        if (!context.Brands.Any())
        {
            await context.Brands.AddRangeAsync(GetPreconfiguredBrands());
            await context.SaveChangesAsync();
        }

        // Seed Suppliers if they don't exist
        if (!context.Suppliers.Any())
        {
            await context.Suppliers.AddRangeAsync(GetPreconfiguredSuppliers());
            await context.SaveChangesAsync();
        }

        // Seed Products if they don't exist
        if (!context.Products.Any())
        {
            await context.Products.AddRangeAsync(GetPreconfiguredProducts(context));
            await context.SaveChangesAsync();
        }
    }

    private static IEnumerable<Category> GetPreconfiguredCategories()
    {
        yield return new Category("Electronics", "0001");
        yield return new Category("Clothing", "0002");
        yield return new Category("Books", "0003");
    }

    private static IEnumerable<Brand> GetPreconfiguredBrands()
    {
        yield return new Brand("Samsung");
        yield return new Brand("Nike");
        yield return new Brand("Penguin");
    }

    private static IEnumerable<Supplier> GetPreconfiguredSuppliers()
    {
        yield return new Supplier("Supplier A");
        yield return new Supplier("Supplier B");
    }

    private static IEnumerable<Product> GetPreconfiguredProducts(CatalogDbContext context)
    {
        var electronicsCategory = context.Categories.FirstOrDefault(c => c.Name == "Electronics");
        var clothingCategory = context.Categories.FirstOrDefault(c => c.Name == "Clothing");
        var booksCategory = context.Categories.FirstOrDefault(c => c.Name == "Books");

        var samsungBrand = context.Brands.FirstOrDefault(b => b.Name == "Samsung");
        var nikeBrand = context.Brands.FirstOrDefault(b => b.Name == "Nike");

        var supplierA = context.Suppliers.FirstOrDefault(s => s.Name == "Supplier A");
        var supplierB = context.Suppliers.FirstOrDefault(s => s.Name == "Supplier B");

        yield return new Product(
            name: "Smartphone",
            size: "Large",
            price: 500.00m,
            priceSale: 450.00m,
            categoryId: electronicsCategory.Id,
            brandId: samsungBrand.Id,
            supplierId: supplierA.Id);

        yield return new Product(
            name: "Laptop",
            size: "Medium",
            price: 1000.00m,
            priceSale: 900.00m,
            categoryId: electronicsCategory.Id,
            brandId: samsungBrand.Id,
            supplierId: supplierB.Id);

        yield return new Product(
            name: "T-shirt",
            size: "Small",
            price: 20.00m,
            priceSale: 15.00m,
            categoryId: clothingCategory.Id,
            brandId: nikeBrand.Id,
            supplierId: supplierA.Id);

        yield return new Product(
            name: "Jeans",
            size: "Large",
            price: 40.00m,
            priceSale: 35.00m,
            categoryId: clothingCategory.Id,
            brandId: nikeBrand.Id,
            supplierId: supplierB.Id);

        yield return new Product(
            name: "Cooking Book",
            size: "N/A",
            price: 25.00m,
            priceSale: 20.00m,
            categoryId: booksCategory.Id,
            brandId: nikeBrand.Id, // No brand for books
            supplierId: supplierA.Id);
    }
}
