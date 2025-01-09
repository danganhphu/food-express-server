using BuildingBlocks.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Catalog.Domain.CategoriesAggregate;

namespace Services.Catalog.Infrastructure.Data.Configuration;

internal sealed class CategoryConfiguration : BaseConfiguration<Category, CategoryId>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.Name).HasMaxLength(DataSchemaLength.Large);
        builder.Property(e => e.Code).HasMaxLength(DataSchemaLength.Small);
    }
}
