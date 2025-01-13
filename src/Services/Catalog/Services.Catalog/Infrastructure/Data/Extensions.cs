using BuildingBlocks.Core.EFCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace Services.Catalog.Infrastructure.Data;

internal static class Extensions
{
    public static IHostApplicationBuilder AddPersistence(this IHostApplicationBuilder builder)
    {
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
                    });
                options.UseSnakeCaseNamingConvention();
            });
        builder.EnrichNpgsqlDbContext<CatalogDbContext>();

        builder.Services.AddMigration<CatalogDbContext, CatalogDbContextSeed>();

        builder.Services.AddKeyedScoped(typeof(IEfRepository<>), "catalog", typeof(CatalogRepository<>));
        builder.Services.AddKeyedScoped(typeof(IEfReadRepository<>), "catalog:read", typeof(CatalogRepository<>));

        return builder;
    }
}
