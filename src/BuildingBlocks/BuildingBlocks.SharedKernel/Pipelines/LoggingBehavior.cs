using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.SharedKernel.Pipelines;

public sealed class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request,
                                        RequestHandlerDelegate<TResponse> next,
                                        CancellationToken cancellationToken)
    {
        const string behavior = nameof(LoggingBehavior<TRequest, TResponse>);

        if (logger.IsEnabled(LogLevel.Information))
        {
        #pragma warning disable CA1848
            logger.LogInformation(
                "[{Behavior}] Handle request={Request} and response={Response}",
                behavior,
                typeof(TRequest).FullName,
                typeof(TResponse).FullName);
        #pragma warning restore CA1848
        }

        var start = Stopwatch.GetTimestamp();

    #pragma warning disable CA1062
        var response = await next().ConfigureAwait(false);
    #pragma warning restore CA1062

    #pragma warning disable CA1848
        logger.LogInformation(
            "[{Behavior}] The request handled {RequestName} with {Response} in {ElapsedMilliseconds} ms",
            behavior,
            typeof(TRequest).Name,
            response,
            Stopwatch.GetElapsedTime(start).Milliseconds);
    #pragma warning restore CA1848

        var timeTaken = Stopwatch.GetElapsedTime(start);

        if (timeTaken.Seconds > 3)
        {
        #pragma warning disable CA1848
            logger.LogWarning(
                "[{Behavior}] The request {Request} took {TimeTaken} seconds.",
                behavior,
                typeof(TRequest).FullName,
                timeTaken.Seconds);
        #pragma warning restore CA1848
        }

        return response;
    }
}
