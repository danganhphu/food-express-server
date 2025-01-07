namespace BuildingBlocks.Core.Queries;

public interface IQuery<out TResponse> : IRequest<TResponse> where TResponse : notnull;
