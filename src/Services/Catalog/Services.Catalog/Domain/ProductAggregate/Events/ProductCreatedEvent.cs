using BuildingBlocks.Core.Domain.Events;

namespace Services.Catalog.Domain.ProductAggregate.Events;

public sealed class ProductCreatedEvent(Product product) : DomainEventBase
{
    
}
