namespace BuildingBlocks.Core.Queries;

public interface ICoreQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : ICoreQuery<TResponse>
    where TResponse : notnull;
