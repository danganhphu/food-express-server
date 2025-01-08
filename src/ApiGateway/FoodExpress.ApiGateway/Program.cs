var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHttpContextAccessor();

builder.Services.AddBff().AddRemoteApis();

builder
    .Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver()
    .AddBffExtensions();

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
        options =>
        {
            options.RequireHttpsMetadata = false;
            options.ClientId = ResourceNameApi.GatewayBff;
            options.ResponseType = OpenIdConnectResponseType.Code;
        });

builder.AddRateLimiting();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseRateLimiter();

app.UseRouting();

app.UseAuthentication();
app.UseBff();

app.MapBffManagementEndpoints();

await app.RunAsync();
