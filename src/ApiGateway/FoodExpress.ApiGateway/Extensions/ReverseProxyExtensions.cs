namespace FoodExpress.ApiGateway.Extensions;

internal static class ReverseProxyExtensions
{
    internal static void AddConfigureReverseProxy(this IHostApplicationBuilder builder, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configuration);

        var reverseProxyConfig = configuration.GetSection("ReverseProxy") ??
                                 throw new ArgumentException("ReverseProxy section is missing!");

        builder
            .Services.AddReverseProxy()
            .LoadFromConfig(reverseProxyConfig)
            .AddTransforms(
                builderContext =>
                {
                    builderContext.AddRequestTransform(
                        async transformContext =>
                        {
                            var accessToken = await transformContext.HttpContext.GetTokenAsync("access_token");

                            if (accessToken != null)
                            {
                                transformContext.ProxyRequest.Headers.Remove("Cookie");
                                transformContext.ProxyRequest.Headers.Authorization =
                                    new("Bearer", accessToken);
                            }
                        });
                })
            .AddServiceDiscoveryDestinationResolver();
    }
}
