namespace BuildingBlocks.Core.Domain.Abstractions;

/// <summary>
/// A simple interface for sending domain events. Can use MediatR or any other implementation.
/// ref: https://github.com/ardalis/Ardalis.SharedKernel/blob/main/src/Ardalis.SharedKernel/IDomainEventDispatcher.cs
/// </summary>
public interface IDomainEventDispatcher
{
    Task DispatchAndClearEvents(IEnumerable<IHasDomainEvents>? entitiesWithEvents);
}
