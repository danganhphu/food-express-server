namespace FoodExpress.ApiGateway.Extensions;

internal static class CacheExtensions
{
    internal static void AddConfigureCache(this IHostApplicationBuilder builder)
    {
        builder.AddRedisDistributedCache(connectionName: ServiceName.Redis);
    }
}
