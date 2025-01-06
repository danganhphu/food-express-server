namespace BuildingBlocks.Core.Domain.Abstractions;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }

    void Delete();
}
