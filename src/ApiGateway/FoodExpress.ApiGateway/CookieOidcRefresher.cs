namespace FoodExpress.ApiGateway;

/// <summary>
/// https://github.com/dotnet/aspnetcore/issues/8175
/// </summary>
/// <param name="oidcOptionsMonitor"></param>
/// <param name="logger"></param>
internal sealed class CookieOidcRefresher(IOptionsMonitor<OpenIdConnectOptions> oidcOptionsMonitor,
                                          ILogger<CookieOidcRefresher> logger)
{
    private readonly OpenIdConnectProtocolValidator _oidcTokenValidator = new()
    {
        // We no longer have the original nonce cookie which is deleted at the end of the authorization code flow having served its purpose.
        // Even if we had the nonce, it's likely expired. It's not intended for refresh requests. Otherwise, we'd use oidcOptions.ProtocolValidator.
        RequireNonce = false,
    };

    public async Task ValidateOrRefreshCookieAsync(CookieValidatePrincipalContext validateContext, string oidcScheme)
    {
        var accessTokenExpirationText = validateContext.Properties.GetTokenValue("expires_at");

        if (!DateTimeOffset.TryParse(accessTokenExpirationText, out var accessTokenExpiration))
        {
            return;
        }

        var oidcOptions = oidcOptionsMonitor.Get(oidcScheme);
        var now = oidcOptions.TimeProvider!.GetUtcNow();

        if (now < accessTokenExpiration)
        {
            return;
        }

        var oidcConfiguration =
            await oidcOptions.ConfigurationManager!.GetConfigurationAsync(validateContext.HttpContext.RequestAborted);
        var tokenEndpoint = oidcConfiguration.TokenEndpoint ??
                            throw new InvalidOperationException("Cannot refresh cookie. TokenEndpoint missing!");

        using var refreshResponse = await oidcOptions.Backchannel.PostAsync(
                                        tokenEndpoint,
                                    #pragma warning disable CA2000
                                        new FormUrlEncodedContent(
                                            new Dictionary<string, string?>()
                                            {
                                                ["grant_type"] = "refresh_token",
                                                ["client_id"] = oidcOptions.ClientId,
                                                ["client_secret"] = oidcOptions.ClientSecret,
                                                ["scope"] = string.Join(" ", oidcOptions.Scope),
                                                ["refresh_token"] =
                                                    validateContext.Properties.GetTokenValue("refresh_token"),
                                            })).ConfigureAwait(false);
    #pragma warning restore CA2000

        if (!refreshResponse.IsSuccessStatusCode)
        {
            logger.LogInformation("Unable to refresh token");
            validateContext.RejectPrincipal();

            return;
        }

        var refreshJson = await refreshResponse.Content.ReadAsStringAsync();
        var message = new OpenIdConnectMessage(refreshJson);

        var validationParameters = oidcOptions.TokenValidationParameters.Clone();

        if (oidcOptions.ConfigurationManager is BaseConfigurationManager baseConfigurationManager)
        {
            validationParameters.ConfigurationManager = baseConfigurationManager;
        }
        else
        {
            validationParameters.ValidIssuer = oidcConfiguration.Issuer;
            validationParameters.IssuerSigningKeys = oidcConfiguration.SigningKeys;
        }

        var validationResult = await oidcOptions.TokenHandler.ValidateTokenAsync(message.IdToken, validationParameters);

        if (!validationResult.IsValid)
        {
            validateContext.RejectPrincipal();

            return;
        }

        var validatedIdToken = JwtSecurityTokenConverter.Convert(validationResult.SecurityToken as JsonWebToken);
        validatedIdToken.Payload["nonce"] = null;
        _oidcTokenValidator.ValidateTokenResponse(
            new OpenIdConnectProtocolValidationContext
            {
                ProtocolMessage = message,
                ClientId = oidcOptions.ClientId,
                ValidatedIdToken = validatedIdToken,
            });

        validateContext.ShouldRenew = true;
        validateContext.ReplacePrincipal(new(validationResult.ClaimsIdentity));

        var expiresIn = int.Parse(message.ExpiresIn, NumberStyles.Integer, CultureInfo.InvariantCulture);
        var expiresAt = now + TimeSpan.FromSeconds(expiresIn);
        validateContext.Properties.StoreTokens(
        [
            new() { Name = "access_token", Value = message.AccessToken },
            new() { Name = "id_token", Value = message.IdToken },
            new() { Name = "token_type", Value = message.TokenType },
            new() { Name = "expires_at", Value = expiresAt.ToString("o", CultureInfo.InvariantCulture) },
        ]);
    #pragma warning disable CA1848
        logger.LogInformation("Token has been refreshed");
    #pragma warning restore CA1848
    }
}
