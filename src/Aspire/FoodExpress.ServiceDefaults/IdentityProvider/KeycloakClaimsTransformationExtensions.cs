using Microsoft.AspNetCore.Authentication;

namespace FoodExpress.ServiceDefaults.IdentityProvider;

public static class KeycloakClaimsTransformationExtensions
{
    /// <summary>
    ///     Adds an <see cref="IClaimsTransformation" /> that transforms Keycloak resource access roles claims into regular
    ///     role claims.
    /// </summary>
    public static void AddKeycloakClaimsTransformation(this IHostApplicationBuilder builder)
    {
        builder.Services.AddTransient<IClaimsTransformation, KeycloakRolesClaimsTransformation>();
    }
}
