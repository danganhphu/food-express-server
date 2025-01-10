using Services.Catalog.Domain.ProductAggregate;
using Services.Catalog.Domain.ProductAggregate.ValueObjects;

namespace Services.Catalog.Infrastructure.Data.Configuration;

internal sealed class ProductConfiguration : BaseConfiguration<Product, ProductId>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.Id)
               .HasConversion(
                   id => id.Value,
                   value => new(value))
               .ValueGeneratedOnAdd();

        builder.Property(x => x.Status)
               .HasConversion(
                   x => x.Value,
                   x => ProductStatus.FromValue(x));

        builder.Property(x => x.Name)
               .HasConversion(
                   x => x.Value,
                   value => new(value))
               .IsRequired(false);

        builder.Property(x => x.Size)
               .HasConversion(
                   x => x.Value,
                   value => new(value))
               .IsRequired(false);

        builder.OwnsOne(
            p => p.Price,
            e =>
            {
                e.ToJson();
                e.Property(x => x.OriginalPrice).IsRequired();
                e.Property(x => x.DiscountPrice).IsRequired(false);
            });

        builder
            .HasOne(x => x.Brand)
            .WithMany()
            .HasForeignKey(x => x.BrandId)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .HasOne(x => x.Supplier)
            .WithMany()
            .HasForeignKey(x => x.SupplierId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Navigation(e => e.Category).AutoInclude();
    }
}
