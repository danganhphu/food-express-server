using BuildingBlocks.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Services.Catalog.Infrastructure.Data.Configuration;

internal abstract class BaseConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity>
    where TEntity : EntityBase<TEntity, TId>
    where TId : notnull
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.Created).IsRequired();

        builder.Property(e => e.CreatedBy);

        builder.Property(e => e.LastModified);

        builder.Property(e => e.LastModified).IsRequired(false);

        builder.Property(e => e.LastModifiedBy).IsRequired(false);

        builder.Property(e => e.Deleted).IsRequired(false);

        builder.Property(e => e.DeletedBy).IsRequired(false);

        builder.Property(e => e.Version).IsConcurrencyToken();

        builder.Property(e => e.IsDeleted).HasDefaultValue(false);

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
