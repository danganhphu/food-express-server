using APIWeaver;

namespace FoodExpress.ServiceDefaults.OpenApi;

public static class OpenApiExtensions
{
    public static void AddDefaultOpenApi(this IServiceCollection services)
    {
        string[] versions = ["v1"];
        foreach (var description in versions)
        {
            services.AddOpenApi(
                description,
                options =>
                {
                    options.AddServerFromRequest();
                    options.ApplyApiVersionInfo();
                    options.ApplySchemaNullableFalse();
                    options.ApplySecuritySchemeDefinitions();
                    options.ApplyOperationDeprecatedStatus();
                }
            );
        }
    }
}
