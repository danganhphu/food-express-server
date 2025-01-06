namespace BuildingBlocks.Core.Domain.Abstractions;

public interface IHaveAudit : IHaveCreator
{
    DateTimeOffset? LastModified { get; }

    Guid? LastModifiedBy { get; }
}
