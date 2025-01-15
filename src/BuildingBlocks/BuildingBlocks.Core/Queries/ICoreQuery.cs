namespace BuildingBlocks.Core.Queries;

public interface ICoreQuery<out TResponse> : IRequest<TResponse> where TResponse : notnull;
