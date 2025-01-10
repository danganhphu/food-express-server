using System;
using Asp.Versioning.Conventions;
using FastEndpoints.AspVersioning;

namespace Services.Catalog.Extensions;

internal static class ApiVersionExtensions
{
    internal static void CreateApiVersion(this IHostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        VersionSets.CreateApi(">>Brands<<", v => v
            .HasApiVersion(1.0));

        VersionSets.CreateApi(">>Categories<<", v => v
            .HasApiVersion(1.0));
        
        VersionSets.CreateApi(">>Suppliers<<", v => v
            .HasApiVersion(1.0));
        
        VersionSets.CreateApi(">>Products<<", v => v
            .HasApiVersion(1.0));
    }
}
