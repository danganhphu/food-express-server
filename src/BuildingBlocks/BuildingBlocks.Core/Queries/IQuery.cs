namespace BuildingBlocks.Core.Queries;

public interface IQuery<out TResponse> : IRequest<TResponse>;
