namespace BuildingBlocks.SharedKernel.EFCore.Interceptors;

public sealed class DeletableInterceptor(IIdentityService currentUser, IDateTimeProvider dateTimeProvider)
    : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
                                                                          InterceptionResult<int> result,
                                                                          CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        UpdateDeletableEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateDeletableEntities(DbContext? context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var utcNow = dateTimeProvider.OffsetUtcNow;
        var userId = currentUser.GetUserId();

        foreach (var entry in context.ChangeTracker.Entries<IHaveDelete>().AsEnumerable()
                                     .Where(entry => entry.State == EntityState.Deleted))
        {
            Extensions.SetPropertyValue(entry, nameof(IHaveDelete.Deleted), utcNow);
            Extensions.SetPropertyValue(entry, nameof(IHaveDelete.DeletedBy), userId);

            entry.State = EntityState.Modified;
        }
    }
}
