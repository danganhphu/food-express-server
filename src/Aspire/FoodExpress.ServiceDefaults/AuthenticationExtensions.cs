namespace FoodExpress.ServiceDefaults;

public static class AuthenticationExtensions
{
    public static IHostApplicationBuilder AddDefaultAuthentication(this IHostApplicationBuilder builder,
                                                                   string realm = "foodexpress")
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddAuthentication()
               .AddKeycloakJwtBearer(
                   ServiceName.IdentityProvider,
                   realm: realm,
                   options =>
                   {
                       options.RequireHttpsMetadata = false;
                       options.Audience = "account";

                       options.TokenValidationParameters.ValidIssuers = [options.Authority];
                   #pragma warning disable CA5404
                       options.TokenValidationParameters.ValidateAudience = false;
                   #pragma warning restore CA5404

                       // Prevent from mapping "sub" claim to nameidentifier.
                       options.MapInboundClaims = false;
                   });
        
        builder.Services.AddAuthorizationBuilder()
               .AddPolicy(
                   Authorization.Policies.Admin,
                   policy => policy.RequireRole(Authorization.Roles.Admin)
               )
               .AddPolicy(Authorization.Policies.User, policy => policy.RequireAuthenticatedUser())
               .SetDefaultPolicy(
                   new AuthorizationPolicyBuilder(Authorization.Policies.User)
                       .RequireAuthenticatedUser()
                       .Build()
               );

        return builder;
    }
}
