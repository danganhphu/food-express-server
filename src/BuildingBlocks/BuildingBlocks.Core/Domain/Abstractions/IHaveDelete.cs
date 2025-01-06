namespace BuildingBlocks.Core.Domain.Abstractions;

public interface IHaveDelete
{
    DateTimeOffset? Deleted { get; set; }

    Guid? DeletedBy { get; set; }
}
