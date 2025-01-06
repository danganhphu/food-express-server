namespace BuildingBlocks.SharedKernel.Identity;

public static class Extensions
{
    public static IHostApplicationBuilder AddConfigureIdentity(this IHostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.AddScoped<IIdentityService, IdentityService>();

        return builder;
    }
}
