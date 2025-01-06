using System.ComponentModel.DataAnnotations.Schema;
using BuildingBlocks.Core.Domain.Abstractions;

namespace BuildingBlocks.Core.Domain.Events;

public abstract class HasDomainEventsBase : IHasDomainEvents
{
    private readonly List<DomainEventBase> _domainEvents = [];

    [NotMapped]
    public IReadOnlyCollection<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

    public void RegisterDomainEvent(DomainEventBase domainEventBase)
        => _domainEvents.Add(domainEventBase);

    public void RemoveDomainEvent(DomainEventBase domainEventBase)
        => _domainEvents.Remove(domainEventBase);

    public void ClearDomainEvents()
        => _domainEvents.Clear();
}
