namespace FoodExpress.ApiGateway.Extensions;

internal static class OauthProxyExtension
{
    public static IHostApplicationBuilder AddOAuthProxy(this IHostApplicationBuilder builder,
                                                        IConfiguration configuration)
    {
        builder.Services.AddSingleton<CookieOidcRefresher>();

        var proxyOptions = configuration.GetSection(nameof(OAuthProxyOptions)).Get<OAuthProxyOptions>();

        if (proxyOptions is null)
            return builder;

        builder.Services.AddAuthentication(
                   options =>
                   {
                       options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                       options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                       options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                   })
               .AddCookie(
                   options =>
                   {
                       options.Cookie.Name = "keycloak.cookie";
                       options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

                       options.SessionStore = new RedisSessionStore(builder.Services);
                   })
               .AddKeycloakOpenIdConnect(
                   ServiceName.IdentityProvider,
                   nameof(FoodExpress),
                   options =>
                   {
                       //Use default signin scheme
                       options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                       options.RequireHttpsMetadata = false;

                       options.ClientId = proxyOptions.ClientId;
                       options.ClientSecret = proxyOptions.ClientSecret;
                       options.ResponseType = OpenIdConnectResponseType.Code;

                       //SameSite is needed for Chrome/Firefox, as they will give http error 500 back, if not set to unspecified.
                       options.NonceCookie.SameSite = SameSiteMode.Unspecified;
                       options.CorrelationCookie.SameSite = SameSiteMode.Unspecified;

                       options.GetClaimsFromUserInfoEndpoint = true;

                       options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();

                       options.Scope.Clear();

                       foreach (var scope in proxyOptions.Scopes)
                       {
                           options.Scope.Add(scope);
                       }

                       options.MapInboundClaims = false;

                       options.TokenValidationParameters = new()
                       {
                           ValidateIssuerSigningKey = true,
                           ValidateTokenReplay = true,
                           NameClaimType = "preferred_username",
                           RoleClaimType = "roles"
                       };

                       options.SaveTokens = true;
                   });

        builder.Services.AddOptions<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme)
               .Configure<CookieOidcRefresher>(
                   (cookieOptions, refresher) =>
                   {
                       cookieOptions.Events.OnValidatePrincipal = context =>
                           refresher.ValidateOrRefreshCookieAsync(context, OpenIdConnectDefaults.AuthenticationScheme);
                   });

        return builder;
    }

    public static IHostApplicationBuilder AddAuthorizationPolicies(this IHostApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder()
               .AddPolicy("authenticatedUser", policy => { policy.RequireAuthenticatedUser(); });

        return builder;
    }
}
