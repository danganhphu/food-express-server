using BuildingBlocks.SharedKernel.ActivityScope;
using FoodExpress.ServiceDefaults.Cors;
using MassTransit.Logging;
using MassTransit.Monitoring;
using OpenTelemetry.Logs;

namespace FoodExpress.ServiceDefaults;

public static class HostingExtensions
{
    public static TBuilder AddServiceDefaults<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.AddCorsPolicy(builder.Configuration);

        builder.ConfigureOpenTelemetry();

        builder.AddDefaultHealthChecks();

        builder.Services.AddServiceDiscovery();

        builder.Services.ConfigureHttpClientDefaults(
            http =>
            {
                // Turn on resilience by default
                http.AddStandardResilienceHandler();

                // Turn on service discovery by default
                http.AddServiceDiscovery();
            });

        return builder;
    }

    private static TBuilder ConfigureOpenTelemetry<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        builder.Logging.AddOpenTelemetry(
            logging =>
            {
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;
            });

        builder.Services.AddOpenTelemetry()
               .WithMetrics(
                   metrics =>
                   {
                       metrics.AddAspNetCoreInstrumentation()
                              .AddHttpClientInstrumentation()
                              .AddProcessInstrumentation()
                              .AddRuntimeInstrumentation()
                              .AddMeter(InstrumentationOptions.MeterName)
                              .AddMeter(ActivitySourceProvider.DefaultSourceName)
                              .AddMeter("Microsoft.SemanticKernel*");
                   })
               .WithTracing(
                   tracing =>
                   {
                       if (builder.Environment.IsDevelopment())
                       {
                           // View all traces in development
                           tracing.SetSampler(new AlwaysOnSampler());
                       }

                       tracing
                           .AddAspNetCoreInstrumentation()
                           .AddHttpClientInstrumentation()
                           .AddGrpcClientInstrumentation()
                           .AddSource(DiagnosticHeaders.DefaultListenerName)
                           .AddSource(ActivitySourceProvider.DefaultSourceName)
                           .AddSource("Microsoft.SemanticKernel*");
                   });

        builder.AddOpenTelemetryExporters();

        return builder;
    }

    private static TBuilder AddOpenTelemetryExporters<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (!useOtlpExporter)
        {
            return builder;
        }

        builder.Services.Configure<OpenTelemetryLoggerOptions>(
            logging =>
                logging.AddOtlpExporter());
        builder.Services.ConfigureOpenTelemetryMeterProvider(
            metrics =>
                metrics.AddOtlpExporter());
        builder.Services.ConfigureOpenTelemetryTracerProvider(
            tracing =>
                tracing.AddOtlpExporter());

        return builder;
    }

    private static TBuilder AddDefaultHealthChecks<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        builder.Services
               .AddHealthChecks()
               .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        return builder;
    }

    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseCorsPolicy();

        if (!app.Environment.IsDevelopment())
            return app;

        // All health checks must pass for app to be considered ready to accept traffic after starting
        app.MapHealthChecks("/health");

        // Only health checks tagged with the "live" tag must pass for app to be considered alive
        app.MapHealthChecks(
            "/alive",
            new()
            {
                Predicate = r => r.Tags.Contains("live")
            });

        return app;
    }
}
