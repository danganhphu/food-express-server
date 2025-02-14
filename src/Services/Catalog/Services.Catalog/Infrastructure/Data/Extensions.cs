using BuildingBlocks.SharedKernel.EFCore.Interceptors;
using BuildingBlocks.SharedKernel.EFCore.Migrations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace Services.Catalog.Infrastructure.Data;

internal static class Extensions
{
    public static IHostApplicationBuilder AddPersistence(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMigration<CatalogDbContext>();

        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, ConcurrencyInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, DeletableInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, PublishDomainEventsInterceptor>();

        //ref: https://github.com/dotnet/aspire/discussions/6967
        //ref: https://github.com/dotnet/docs-aspire/issues/1601#issuecomment-2333380717

        builder.Services.AddDbContext<CatalogDbContext>(
            (sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseNpgsql(
                           builder.Configuration.GetConnectionString(ServiceName.Catalog),
                           npgsqlOptionsAction: optionsAction =>
                           {
                               optionsAction.MigrationsAssembly("FoodExpress.Migrations");
                               optionsAction.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                           })
                       .UseSnakeCaseNamingConvention()
                       .ConfigureWarnings(
                           warnings =>
                               warnings.Ignore(RelationalEventId.PendingModelChangesWarning))
                       .UseAsyncSeeding(
                           async (context, _, cancellationToken) =>
                           {
                               if (builder.Environment.IsProduction())
                               {
                                   return;
                               }

                               if (
                                   !await context
                                          .Set<Category>()
                                          .AnyAsync(cancellationToken: cancellationToken)
                               )
                               {
                                   var categoryFaker = new Faker<Category>()
                                                       .UseSeed(Seeder.DefaultSeed)
                                                       .CustomInstantiator(
                                                           f => new(
                                                               f.Commerce.Categories(1).FirstOrDefault(),
                                                               f.Random.AlphaNumeric(4)));

                                   var categoriesToSeed = categoryFaker.Generate(Seeder.DefaultAmount);

                                   context.Set<Category>().AddRange(categoriesToSeed);
                                   await context.SaveChangesAsync(cancellationToken);
                               }

                               if (
                                   !await context
                                          .Set<Brand>()
                                          .AnyAsync(cancellationToken: cancellationToken)
                               )
                               {
                                   var brandFaker = new Faker<Brand>()
                                                    .UseSeed(Seeder.DefaultSeed)
                                                    .CustomInstantiator(f => new(f.Company.CompanyName()));
                                   var brandsToSeed = brandFaker.Generate(Seeder.DefaultAmount);

                                   context.Set<Brand>().AddRange(brandsToSeed);
                                   await context.SaveChangesAsync(cancellationToken);
                               }

                               if (
                                   !await context
                                          .Set<Supplier>()
                                          .AnyAsync(cancellationToken: cancellationToken)
                               )
                               {
                                   var supplierFaker = new Faker<Supplier>()
                                                       .UseSeed(Seeder.DefaultSeed)
                                                       .CustomInstantiator(f => new(f.Company.CompanyName()));

                                   var suppliersToSeed = supplierFaker.Generate(Seeder.DefaultAmount);

                                   context.Set<Supplier>().AddRange(suppliersToSeed);
                                   await context.SaveChangesAsync(cancellationToken);
                               }

                               if (!await context.Set<Product>().AnyAsync(cancellationToken: cancellationToken))
                               {
                                   var categories = await context.Set<Category>().ToListAsync(cancellationToken);
                                   var brands = await context.Set<Brand>().ToListAsync(cancellationToken);
                                   var suppliers = await context.Set<Supplier>().ToListAsync(cancellationToken);

                                   if (categories.Count == 0 || brands.Count == 0 || suppliers.Count == 0)
                                   {
                                       throw new InvalidOperationException(
                                           "Seeding failed: Ensure Categories, Brands, and Suppliers are seeded first.");
                                   }

                                   var productFaker = new Faker<Product>()
                                                      .UseSeed(Seeder.DefaultSeed)
                                                      .CustomInstantiator(
                                                          f =>
                                                          {
                                                              var category = f.PickRandom(categories);
                                                              var brand = f.PickRandom(brands);
                                                              var supplier = f.PickRandom(suppliers);

                                                              return new Product(
                                                                  f.Commerce.ProductName(),
                                                                  f.PickRandom("S", "M", "L", "XL"),
                                                                  f.Random.Decimal(10, 500),
                                                                  f.Random.Decimal(5, 450),
                                                                  new(category.Id.Value),
                                                                  new(brand.Id.Value),
                                                                  new(supplier.Id.Value));
                                                          });

                                   var productsToSeed = productFaker.Generate(Seeder.DefaultLargeAmount);
                                   context.Set<Product>().AddRange(productsToSeed);
                                   await context.SaveChangesAsync(cancellationToken);
                               }
                           });
            });

        builder.EnrichNpgsqlDbContext<CatalogDbContext>();

        builder.Services.AddScoped<IDatabaseFacade>(p => p.GetRequiredService<CatalogDbContext>());

        builder.Services.AddKeyedScoped(typeof(IEfRepository<>), "catalog", typeof(CatalogRepository<>));
        builder.Services.AddKeyedScoped(typeof(IEfReadRepository<>), "catalog:read", typeof(CatalogRepository<>));

        return builder;
    }
}
