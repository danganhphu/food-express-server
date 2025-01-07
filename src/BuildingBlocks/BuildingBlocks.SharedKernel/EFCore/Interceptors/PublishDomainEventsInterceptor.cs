using BuildingBlocks.Core.Domain;
using BuildingBlocks.Core.Domain.Events;
using MediatR;

namespace BuildingBlocks.SharedKernel.EFCore.Interceptors;

public sealed class PublishDomainEventsInterceptor(IPublisher publisher)
    : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        await PublishDomainEventsAsync(eventData.Context);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task PublishDomainEventsAsync(DbContext? context)
    {
        if (context is null)
            return;

        var domainEntities = context
                             .ChangeTracker.Entries<HasDomainEventsBase>()
                             .Where(x => x.Entity.DomainEvents.Count != 0)
                             .ToImmutableList();

        var domainEvents = domainEntities.SelectMany(x => x.Entity.DomainEvents).ToImmutableList();

        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent).ConfigureAwait(false);
        }
    }
}
