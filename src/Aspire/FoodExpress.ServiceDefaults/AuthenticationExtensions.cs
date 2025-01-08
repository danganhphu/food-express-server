using BuildingBlocks.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FoodExpress.ServiceDefaults;

public static class AuthenticationExtensions
{
    public static IHostApplicationBuilder AddDefaultAuthentication(this IHostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var configuration = builder.Configuration;
        var identity = configuration.GetSection(nameof(Identity)).Get<Identity>();

        if (identity is null)
        {
            return builder;
        }

        // JsonWebTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

        builder.Services.AddAuthentication()
               .AddKeycloakJwtBearer(
                   ServiceName.IdentityProvider,
                   realm: nameof(FoodExpress),
                   options =>
                   {
                       options.RequireHttpsMetadata = false;
                       options.Audience = identity.Audience;

                       options.TokenValidationParameters.ValidIssuers = [options.Authority];
                   #pragma warning disable CA5404
                       options.TokenValidationParameters.ValidateAudience = false;
                   #pragma warning restore CA5404

                       // Prevent from mapping "sub" claim to nameidentifier.
                       options.MapInboundClaims = false;
                   });

        builder.Services.AddAuthorization();

        return builder;
    }
}
