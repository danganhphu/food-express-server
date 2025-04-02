using Saunter;

namespace FoodExpress.ServiceDefaults;

public static class AsyncApiExtensions
{
    public static void AddAsyncApiDocs(
        this IServiceCollection services,
        IList<Type> types,
        string serviceName
    )
    {
        if (!serviceName.EndsWith("Service"))
        {
            serviceName += "Service";
        }

        services.AddAsyncApiSchemaGeneration(options =>
        {
            options.AssemblyMarkerTypes = types;

            string[] versions = ["1.0.0"];

            foreach (var version in versions)
            {
                options.AsyncApi = new()
                {
                    Info = new(serviceName, version)
                    {
                        License = new("MIT") { Url = new("https://opensource.org/licenses/MIT") },
                        Contact = new()
                        {
                            Name = "Phu Dang",
                            Url = new("https://github.com/danganhphu"),
                        },
                    },
                };
            }
        });
    }

    public static void MapAsyncApi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapAsyncApiDocuments();
        endpoints.MapAsyncApiUi();
    }
}
