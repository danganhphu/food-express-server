namespace BuildingBlocks.Core.Commands;

/// <summary>
/// 
/// </summary>
public interface ICommand : ICommand<Unit>;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface ICommand<out TResponse> : IRequest<TResponse> where TResponse : notnull;
