namespace BuildingBlocks.Core.Domain;

/// <summary>
/// ref: https://github.com/ardalis/Ardalis.SharedKernel/blob/main/src/Ardalis.SharedKernel/EntityBase.cs
/// For use with Vogen or similar tools for generating code for 
/// strongly typed Ids.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TId"></typeparam>
public abstract class EntityBase<T, TId> : HasDomainEventsBase, IHaveVersion, IHaveAudit, IHaveDelete, ISoftDelete
    where T : EntityBase<T, TId>
    where TId : notnull
{
    public TId Id { get; private set; } = default!;

    public Guid Version { get; set; }

    public DateTimeOffset Created { get; set; }
    public Guid CreatedBy { get; set; }

    public DateTimeOffset? LastModified { get; set; }
    public Guid? LastModifiedBy { get; set; }

    public DateTimeOffset? Deleted { get; set; }
    public Guid? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }

    public void Delete()
        => IsDeleted = true;
}
