namespace BuildingBlocks.Core.Domain.Abstractions;

public interface IHasDomainEvents
{
    IReadOnlyCollection<DomainEventBase> DomainEvents { get; }
}
