namespace BuildingBlocks.Core.Domain.Abstractions;

public interface IHaveCreator
{
    DateTimeOffset Created { get; }
    Guid CreatedBy { get; }
}
