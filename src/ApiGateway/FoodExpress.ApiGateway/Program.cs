using FoodExpress.ApiGateway;
using FoodExpress.ServiceDefaults;
using FoodExpress.ServiceDefaults.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHttpContextAccessor();

builder.Services.AddBff().AddRemoteApis();

builder
    .Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver()
    .AddBffExtensions();

Configuration config = new();
builder.Configuration.Bind("BFF", config);

builder
    .Services.AddAuthentication(
        options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            options.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
    .AddCookie(
        CookieAuthenticationDefaults.AuthenticationScheme,
        options =>
        {
            options.Cookie.Name = "__Host-bff";
            options.Cookie.SameSite = SameSiteMode.Strict;
        })
    .AddKeycloakOpenIdConnect(
        ServiceName.IdentityProvider,
        nameof(FoodExpress),
        OpenIdConnectDefaults.AuthenticationScheme,
        options =>
        {
            options.ClientId = config.ClientId;
            options.ClientSecret = config.ClientSecret;
            options.UsePkce = true;

            options.ResponseType = OpenIdConnectResponseType.Code;
            options.ResponseMode = OpenIdConnectResponseMode.Query;

            options.GetClaimsFromUserInfoEndpoint = true;
            options.MapInboundClaims = false;
            options.SaveTokens = true;

            options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();

            options.Scope.Clear();

            foreach (var scope in config.Scopes)
            {
                options.Scope.Add(scope);
            }

            options.TokenValidationParameters.ValidIssuers = [config.Authority];
        #pragma warning disable CA5404
            options.TokenValidationParameters.ValidateAudience = false;
        #pragma warning restore CA5404
        });

builder.AddRateLimiting();

builder.AddDefaultOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseRateLimiter();

app.UseAuthentication();
app.UseBff();
app.UseRouting();
app.UseAuthorization();

app.MapBffManagementEndpoints();
app.MapBffReverseProxy();

if (config.Apis.Count != 0)
{
    foreach (var api in config.Apis)
    {
        app.MapRemoteBffApiEndpoint(api.LocalPath, api.RemoteUrl!)
           .RequireAccessToken(api.RequiredToken);
    }
}
await app.RunAsync();
