namespace FoodExpress.AppHost;

internal static class Extensions
{
    internal static IDistributedApplicationBuilder AddForwardedHeaders(this IDistributedApplicationBuilder builder)
    {
        builder.Services.TryAddLifecycleHook<AddForwardHeadersHook>();

        return builder;
    }

    internal sealed class AddForwardHeadersHook : IDistributedApplicationLifecycleHook
    {
        public Task BeforeStartAsync(DistributedApplicationModel appModel,
                                     CancellationToken cancellationToken = default)
        {
            foreach (var p in appModel.GetProjectResources())
            {
                p.Annotations.Add(
                    new EnvironmentCallbackAnnotation(
                        context =>
                        {
                            context.EnvironmentVariables["ASPNETCORE_FORWARDEDHEADERS_ENABLED"] =
                                "true";
                        }));
            }

            return Task.CompletedTask;
        }
    }
}
