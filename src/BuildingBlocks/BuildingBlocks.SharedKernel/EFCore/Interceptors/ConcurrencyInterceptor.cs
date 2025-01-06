namespace BuildingBlocks.SharedKernel.EFCore.Interceptors;

public sealed class ConcurrencyInterceptor
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

        ApplyConcurrencyVersionEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void ApplyConcurrencyVersionEntities(DbContext? context)
    {
        if (context == null)
            return;

        var version = GuidIdGenerator.NewGuid();

        foreach (var entry in context.ChangeTracker.Entries<IHaveVersion>().Where(
                     entry =>
                         entry.State == EntityState.Modified))
        {
            Extensions.SetPropertyValue(entry, nameof(IHaveVersion.Version), version);
        }
    }
}
