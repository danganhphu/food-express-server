namespace BuildingBlocks.SharedKernel.EFCore.Interceptors;

public static class Extensions
{
    /// <summary>
    /// Sets the current value of a property on an entity entry, ensuring it is not a primary key.
    /// </summary>
    /// <param name="entry">The EntityEntry for the entity being updated.</param>
    /// <param name="propertyName">The name of the property to update.</param>
    /// <param name="value">The value to set for the property.</param>
    public static void SetPropertyValue(EntityEntry entry, string propertyName, object value)
    {
        var property = entry.Property(propertyName);
        if (!property.Metadata.IsPrimaryKey())
        {
            property.CurrentValue = value;
        }
    }
    
    // public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
    //     entry.References.Any(r =>
    //         r.TargetEntry != null &&
    //         r.TargetEntry.Metadata.IsOwned() &&
    //         r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}
