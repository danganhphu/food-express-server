var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHttpContextAccessor();

builder.AddDefaultAuthentication();

builder.Services.AddRequestTimeouts(
    options =>
        options.AddPolicy("timeout-1-minute", TimeSpan.FromMinutes(1)));

builder.AddRateLimiting();

builder
    .Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();

var app = builder.Build();

app.UseRequestTimeouts();

app.UseRateLimiter();

app.MapDefaultEndpoints();

app.MapReverseProxy();

await app.RunAsync();
