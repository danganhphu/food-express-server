namespace FoodExpress.ServiceDefaults.Cors;

internal static class Extensions
{
    private const string CorsPolicy = nameof(CorsPolicy);

    internal static TBuilder AddCorsPolicy<TBuilder>(this TBuilder builder, IConfiguration config)
        where TBuilder : IHostApplicationBuilder
    {
        var corsOptions = config.GetSection(nameof(CorsOptions)).Get<CorsOptions>();

        if (corsOptions == null)
            return builder;

        builder.Services.AddCors(
            opt =>
                opt.AddPolicy(
                    CorsPolicy,
                    policy =>
                        policy.AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials()
                              .WithOrigins(corsOptions.AllowedOrigins.ToArray())));

        return builder;
    }

    internal static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app)
        => app.UseCors(CorsPolicy);
}
