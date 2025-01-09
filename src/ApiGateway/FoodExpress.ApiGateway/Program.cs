var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHttpContextAccessor();

builder.AddConfigureReverseProxy(builder.Configuration);

builder.AddOAuthProxy(builder.Configuration);

builder.AddAuthorizationPolicies();

builder.AddConfigureCache();

builder.AddRateLimiting();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseRateLimiter();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/welcome", () => Results.Ok("Welcome to API Gateway"));

// Login endpoint
app.MapGet(
    "/account/login",
    [Authorize](HttpContext context) => Results.Redirect("/welcome"));

// Info endpoint
app.MapGet(
    "/account/info",
    (HttpContext context) =>
    {
        var claims = context.User.Claims.Select(x => new { x.Type, x.Value }).ToList();

        return Results.Ok(claims);
    });

// Logout endpoint
app.MapGet(
    "/account/logout",
    [Authorize] async (HttpContext context) =>
    {
        var prop = new AuthenticationProperties
        {
            RedirectUri = "/account/public"
        };

        await context.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, prop);
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Results.Redirect("/welcome");
    });

app.MapReverseProxy();


await app.RunAsync();
