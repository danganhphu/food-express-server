using BuildingBlocks.SharedKernel.Clock;
using FoodExpress.ServiceDefaults;

namespace Services.Ordering.Api.Extensions;

internal static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddDefaultAuthentication();
        builder.Services.AddAuthorizationBuilder();
        builder.Services.AddHttpContextAccessor();

        builder.AddDateTimeProvider();
    }

    public static IApplicationBuilder UseApplicationServices(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
