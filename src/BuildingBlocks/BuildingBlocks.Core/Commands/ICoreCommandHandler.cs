namespace BuildingBlocks.Core.Commands;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public interface ICoreCommandHandler<in TCommand> : ICoreCommandHandler<TCommand, Unit>
    where TCommand : ICoreCommand<Unit>;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface ICoreCommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICoreCommand<TResponse>
    where TResponse : notnull;
