using BuildingBlocks.Constants;
using Microsoft.EntityFrameworkCore;

namespace Services.Catalog.Infrastructure.Data;

public sealed class CatalogContext(DbContextOptions<CatalogContext> options)
    : DbContext(options)

{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ICatalogAssemblyMaker).Assembly);
        modelBuilder.HasDefaultSchema(SchemaNames.Catalog);
    }
}
