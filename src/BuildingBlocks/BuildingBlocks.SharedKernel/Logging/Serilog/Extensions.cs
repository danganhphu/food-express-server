namespace BuildingBlocks.SharedKernel.Logging.Serilog;

public static class Extensions
{
    public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Host.UseSerilog(
            (context, logger) =>
            {
                logger.ReadFrom.Configuration(context.Configuration);
                logger.Enrich.FromLogContext();
                logger.Enrich.WithCorrelationId();
                logger
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
                    .Filter.ByExcluding(
                        Matching.FromSource("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware"));
            });

        return builder;
    }
}
