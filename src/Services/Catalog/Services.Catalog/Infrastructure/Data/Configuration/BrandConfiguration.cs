using BuildingBlocks.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Catalog.Domain.BrandAggregate;

namespace Services.Catalog.Infrastructure.Data.Configuration;

internal sealed class BrandConfiguration : BaseConfiguration<Brand, BrandId>
{
    public override void Configure(EntityTypeBuilder<Brand> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.Name).HasMaxLength(DataSchemaLength.Medium);
    }
}
