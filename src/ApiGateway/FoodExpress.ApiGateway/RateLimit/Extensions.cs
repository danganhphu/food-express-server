namespace FoodExpress.ApiGateway.RateLimit;

internal static class RateLimitExtensions
{
    private const string PerIpPolicy = "fixed-by-ip";

    public static IHostApplicationBuilder AddRateLimiting(this IHostApplicationBuilder builder)
    {
        builder.Services.AddRateLimiter(
            options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.AddPolicy(
                    PerIpPolicy,
                    httpContext =>
                        RateLimitPartition.GetFixedWindowLimiter(
                            httpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty,
                            _ => new() { PermitLimit = 60, Window = TimeSpan.FromMinutes(1) }));
            });

        return builder;
    }
}
