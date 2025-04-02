using FoodExpress.ServiceDefaults.Cors;
using FoodExpress.ServiceDefaults.IdentityProvider;
using FoodExpress.ServiceDefaults.OpenApi;

namespace Services.Catalog.Extensions;

public static class Extensions
{
    public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.CreateApiVersion();

        builder.AddCorsPolicy(builder.Configuration);
        
        builder.Services.AddDefaultOpenApi();

        // builder.AddDefaultAuthentication().AddKeycloakClaimsTransformation();
        builder.AddDefaultAuthentication();

        builder.Services.AddHttpContextAccessor();

        builder.AddDateTimeProvider();

        builder.AddConfigureIdentity();

        builder.AddConfigureFastEndpoints();

        builder.AddPersistence();

        builder.Services.AddMediatR(
            cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<ICatalogAssemblyMaker>();
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
                cfg.AddOpenBehavior(typeof(MetricsBehavior<,>));
            });

        builder.Services.AddSingleton<IActivityScope, ActivityScope>();
        builder.Services.AddSingleton<CommandHandlerMetrics>();
        builder.Services.AddSingleton<QueryHandlerMetrics>();
        
        return builder;
    }

    public static IApplicationBuilder UseApplicationServices(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseCorsPolicy();
        
        app.MapOpenApi();
        
        app.UseAuthentication()
           .UseAuthorization()
           .UseDefaultExceptionHandler(app.Logger, true, true)
           .UseFastEndpoints(
               c =>
               {
                   c.Endpoints.RoutePrefix = "api";
                   c.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                   c.Errors.UseProblemDetails();
               });

        app.UseStatusCodePages();

        return app;
    }
}
