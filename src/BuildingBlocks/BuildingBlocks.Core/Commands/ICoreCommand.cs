namespace BuildingBlocks.Core.Commands;

/// <summary>
/// 
/// </summary>
public interface ICoreCommand : ICoreCommand<Unit>;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface ICoreCommand<out TResponse> : IRequest<TResponse> where TResponse : notnull;
