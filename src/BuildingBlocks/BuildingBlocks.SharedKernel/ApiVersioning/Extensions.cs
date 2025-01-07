using Asp.Versioning;
using FastEndpoints;
using FastEndpoints.AspVersioning;

namespace BuildingBlocks.SharedKernel.ApiVersioning;

public static class Extensions
{
    public static IHostApplicationBuilder AddConfigureFastEndpoints(this IHostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.AddFastEndpoints().AddVersioning(
            options =>
            {
                options.DefaultApiVersion = new(1, 0);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version"));
            });

        return builder;
    }
}
