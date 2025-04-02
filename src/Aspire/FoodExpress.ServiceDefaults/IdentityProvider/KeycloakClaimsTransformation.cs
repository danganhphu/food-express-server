using Microsoft.Extensions.Options;

namespace FoodExpress.ServiceDefaults.IdentityProvider;

public sealed class KeycloakRolesClaimsTransformation(IOptionsSnapshot<JwtBearerOptions> jwtBearerOptions)
    : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var options = jwtBearerOptions.Get(JwtBearerDefaults.AuthenticationScheme);
        var clientId =
            options.TokenValidationParameters.ValidAudience ??
            options.TokenValidationParameters.ValidAudiences.FirstOrDefault() ??
            throw new InvalidOperationException("Audience is not set on JwtBearerOptions");

        if (!principal.TryGetJsonClaim("resource_access", out var resourceAccess))
        {
            return Task.FromResult(principal);
        }

        if (!principal.TryGetJsonClaim("realm_access", out var realmAccess))
        {
            return Task.FromResult(principal);
        }

        if (
            resourceAccess[clientId] is not JsonObject resourceNode ||
            resourceNode["roles"] is not JsonArray resourceRoles
        )
        {
            return Task.FromResult(principal);
        }

        if (realmAccess["roles"] is not JsonArray realmRoles)
        {
            return Task.FromResult(principal);
        }

        var claimsIdentity = new ClaimsIdentity();

        // Convert resource roles to regular roles.
        foreach (var role in resourceRoles.GetValues<string>())
        {
            if (!claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == role))
            {
                claimsIdentity.AddClaim(new(ClaimTypes.Role, role));
            }
        }

        // Convert realm roles to regular roles.
        foreach (var role in realmRoles.GetValues<string>())
        {
            if (!claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == role))
            {
                claimsIdentity.AddClaim(new(ClaimTypes.Role, role));
            }
        }

        principal.AddIdentity(claimsIdentity);

        return Task.FromResult(principal);
    }
}
