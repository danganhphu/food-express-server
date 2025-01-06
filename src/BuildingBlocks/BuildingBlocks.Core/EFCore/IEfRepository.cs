namespace BuildingBlocks.Core.EFCore;

public interface IEfRepository<T> : IRepositoryBase<T>
    where T : class, IAggregateRoot;
