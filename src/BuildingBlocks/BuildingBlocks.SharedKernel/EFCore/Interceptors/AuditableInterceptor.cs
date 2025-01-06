namespace BuildingBlocks.SharedKernel.EFCore.Interceptors;

public sealed class AuditableInterceptor(IIdentityService currentUser, IDateTimeProvider dateTimeProvider)
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

        UpdateAuditableEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditableEntities(DbContext? context)
    {
        if (context == null)
            return;

        var utcNow = dateTimeProvider.OffsetUtcNow;
        var userId = currentUser.GetUserId();

        foreach (var entry in context.ChangeTracker.Entries<IHaveAudit>().AsEnumerable().Where(
                     entry =>
                         entry.State is EntityState.Added or EntityState.Modified))
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    Extensions.SetPropertyValue(entry, nameof(IHaveCreator.Created), utcNow);
                    Extensions.SetPropertyValue(entry, nameof(IHaveCreator.CreatedBy), userId);

                    break;

                case EntityState.Modified:
                    Extensions.SetPropertyValue(entry, nameof(IHaveAudit.LastModified), utcNow);
                    Extensions.SetPropertyValue(entry, nameof(IHaveAudit.LastModifiedBy), userId);

                    break;
            }
        }
    }
}
