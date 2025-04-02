namespace FoodExpress.ServiceDefaults;

public static class HostingExtensions
{
    private const string HealthChecks = nameof(HealthChecks);

    public static TBuilder AddServiceDefaults<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
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

    private static void ConfigureOpenTelemetry<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        builder.Logging.EnableEnrichment();
        builder.Services.AddHttpContextAccessor();
        // builder.Services.AddLogEnricher<ApplicationEnricher>();
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
                           .AddSource(builder.Environment.ApplicationName)
                           .AddAspNetCoreInstrumentation()
                           .AddHttpClientInstrumentation()
                           .AddGrpcClientInstrumentation()
                           .AddSource(DiagnosticHeaders.DefaultListenerName)
                           .AddSource(ActivitySourceProvider.DefaultSourceName)
                           .AddSource("Microsoft.SemanticKernel*");
                   });

        builder.AddOpenTelemetryExporters();
    }

    private static void AddOpenTelemetryExporters<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (!useOtlpExporter)
        {
            return;
        }

        builder.Services.AddOpenTelemetry().UseOtlpExporter();
    }

    private static void AddDefaultHealthChecks<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        var healthChecksConfiguration = builder.Configuration.GetSection(HealthChecks);

        // All health checks endpoints must return within the configured timeout value (defaults to 5 seconds)
        var healthChecksRequestTimeout =
            healthChecksConfiguration.GetValue<TimeSpan?>("RequestTimeout") ?? TimeSpan.FromSeconds(5);
        builder.Services.AddRequestTimeouts(
            timeouts =>
                timeouts.AddPolicy(HealthChecks, healthChecksRequestTimeout));

        // Cache health checks responses for the configured duration (defaults to 10 seconds)
        var healthChecksExpireAfter =
            healthChecksConfiguration.GetValue<TimeSpan?>("ExpireAfter") ?? TimeSpan.FromSeconds(10);
        builder.Services.AddOutputCache(
            caching =>
                caching.AddPolicy(HealthChecks, policy => policy.Expire(healthChecksExpireAfter)));

        builder.Services
               .AddHealthChecks()
               .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);
    }

    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        if (!app.Environment.IsDevelopment())
            return app;

        var healthChecksUrls = app.Configuration["HEALTHCHECKSUI_URLS"];

        if (string.IsNullOrWhiteSpace(healthChecksUrls))
            return app;

        var healthChecks = app.MapGroup("");

        // Configure health checks endpoints to use the configured request timeouts and cache policies
        healthChecks.CacheOutput(HealthChecks).WithRequestTimeout(HealthChecks);

        // All health checks must pass for app to be considered ready to accept traffic after starting
        app.MapHealthChecks("/health");

        // Only health checks tagged with the "live" tag must pass for app to be considered alive
        app.MapHealthChecks(
            "/alive",
            new()
            {
                Predicate = r => r.Tags.Contains("live")
            });

        var pathToHostsMap = GetPathToHostsMap(healthChecksUrls);

        foreach (var path in pathToHostsMap.Keys)
        {
            // Ensure that the HealthChecksUI endpoint is only accessible from configured hosts, e.g. localhost:12345, hub.docker.internal, etc.
            // as it contains more detailed information about the health of the app including the types of dependencies it has.

            healthChecks
                .MapHealthChecks(
                    path,
                    new() { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse })

                // This ensures that the HealthChecksUI endpoint is only accessible from the configured health checks URLs.
                // See this documentation to learn more about restricting access to health checks endpoints via routing:
                // https://learn.microsoft.com/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-8.0#use-health-checks-routing
                .RequireHost(pathToHostsMap[path]);
        }

        // Redirect the root path to the health checks endpoint
        healthChecks.MapGet(
            "/",
            async context =>
            {
                context.Response.Redirect("/health");
                await Task.CompletedTask;
            });

        return app;
    }

    private static Dictionary<string, string[]> GetPathToHostsMap(string healthChecksUrls)
    {
        // Given a value like "localhost:12345/healthz;hub.docker.internal:12345/healthz" return a dictionary like:
        // { { "healthz", [ "localhost:12345", "hub.docker.internal:12345" ] } }

        var uris = healthChecksUrls
                   .Split(';', StringSplitOptions.RemoveEmptyEntries)
                   .Select(url => new Uri(url, UriKind.Absolute))
                   .GroupBy(uri => uri.AbsolutePath, uri => uri.Authority)
                   .ToDictionary(g => g.Key, g => g.ToArray());

        return uris;
    }
}
