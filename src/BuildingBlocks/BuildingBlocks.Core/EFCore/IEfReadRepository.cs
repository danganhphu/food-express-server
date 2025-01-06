namespace BuildingBlocks.Core.EFCore;

public interface IEfReadRepository<T> : IReadRepositoryBase<T>
    where T : class, IAggregateRoot;
