namespace BuildingBlocks.Core.Domain.Abstractions;

public interface IHaveVersion
{
    [ConcurrencyCheck]
    public Guid Version { get; set; }
}
