using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Core.Domain.Events;

public sealed class DomainEventDispatcher(IPublisher publisher, ILogger<DomainEventDispatcher> logger)
    : IDomainEventDispatcher
{
    public async Task DispatchAndClearEvents(IEnumerable<IHasDomainEvents>? entitiesWithEvents)
    {
        if (entitiesWithEvents != null)
        {
            foreach (var entity in entitiesWithEvents)
            {
                if (entity is HasDomainEventsBase hasDomainEvents)
                {
                    var events = hasDomainEvents.DomainEvents.ToArray();
                    hasDomainEvents.ClearDomainEvents();

                    foreach (var domainEvent in events)
                        await publisher.Publish(domainEvent).ConfigureAwait(false);
                }
                else
                {
                #pragma warning disable CA1848
                    logger.LogError(
                        "Entity of type {EntityType} does not inherit from {BaseType}. Unable to clear domain events.",
                        entity.GetType().Name,
                        nameof(HasDomainEventsBase));
                #pragma warning restore CA1848
                }
            }
        }
    }
}
