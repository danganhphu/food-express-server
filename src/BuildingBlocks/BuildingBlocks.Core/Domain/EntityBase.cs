using BuildingBlocks.Core.Domain.Abstractions;
using BuildingBlocks.Core.Domain.Events;

namespace BuildingBlocks.Core.Domain;

/// <summary>
/// For use with Vogen or similar tools for generating code for 
/// strongly typed Ids.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TId"></typeparam>
public abstract class EntityBase<T, TId> : HasDomainEventsBase, IHaveVersion, IHaveAudit, IHaveDelete
    where T : EntityBase<T, TId>
    where TId : notnull
{
    public TId Id { get; set; } = default!;

    public Guid Version { get; set; }

    public DateTimeOffset Created { get; set; }
    public Guid CreatedBy { get; set; }

    public DateTimeOffset? LastModified { get; set; }
    public Guid? LastModifiedBy { get; set; }

    public DateTimeOffset? Deleted { get; set; }
    public Guid? DeletedBy { get; set; }
}
