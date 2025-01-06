using BuildingBlocks.Core.IdsGenerator;

namespace BuildingBlocks.Core.Domain;

public abstract class EntityBase<TId> : HasDomainEventsBase
    where TId : struct, IEquatable<TId>
{
    public TId Id { get; protected init; }
}

public abstract class EntityBase : EntityBase<Guid>, IHaveVersion, IHaveAudit, IHaveDelete
{
    protected EntityBase()
    {
        Id = GuidIdGenerator.NewGuid();
    }

    public Guid Version { get; set; }

    public DateTimeOffset Created { get; set; }
    public Guid CreatedBy { get; set; }

    public DateTimeOffset? LastModified { get; set; }
    public Guid? LastModifiedBy { get; set; }

    public DateTimeOffset? Deleted { get; set; }
    public Guid? DeletedBy { get; set; }
}
