namespace BuildingBlocks.SharedKernel.Clock;

public static class Extensions
{
    public static IHostApplicationBuilder AddDateTimeProvider(this IHostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        _ = builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return builder;
    }
}
