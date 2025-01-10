namespace Services.Catalog.Infrastructure.Data.Configuration;

internal sealed class CategoryConfiguration : BaseConfiguration<Category, CategoryId>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.Id)
               .HasConversion(
                   id => id.Value,
                   value => new(value))
               .ValueGeneratedOnAdd();

        builder.Property(e => e.Name).HasMaxLength(DataSchemaLength.Large);
        builder.Property(e => e.Code).HasMaxLength(DataSchemaLength.Small);
    }
}
