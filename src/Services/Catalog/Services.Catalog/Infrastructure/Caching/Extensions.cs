namespace Services.Catalog.Infrastructure.Caching;

public static class Extensions
{
    public static IHostApplicationBuilder AddConfigureHybridCache(this IHostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

    #pragma warning disable EXTEXP0018
        builder.Services.AddHybridCache(
            options =>
            {
                options.DefaultEntryOptions = new()
                {
                    Expiration = TimeSpan.FromMinutes(5),
                    LocalCacheExpiration = TimeSpan.FromMinutes(5)
                };
            });
    #pragma warning restore EXTEXP0018

        builder.AddRedisDistributedCache(connectionName: ServiceName.Redis);

        return builder;
    }
}
