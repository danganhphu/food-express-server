using FoodExpress.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddForwardedHeaders();

var launchProfileName = builder.Configuration["DOTNET_LAUNCH_PROFILE"] ?? "http";

// Services
var identityProvider = builder.AddKeycloak(ServiceName.IdentityProvider, 18080)
                              .WithExternalHttpEndpoints()
                              .WithDataVolume();

if (builder.Environment.IsDevelopment())
{
    identityProvider.WithRealmImport("./Keycloak/");
}

var identityEndpoint = identityProvider.GetEndpoint(launchProfileName);

var orderingApi = builder.AddProject<Projects.Services_Ordering_Api>(ResourceNameApi.Order)
                         .WithReference(identityProvider)
                         .WaitFor(identityProvider)
                         .WithEnvironment(EnvironmentNameService.Identity, identityEndpoint);

var gateway = builder.AddProject<Projects.FoodExpress_ApiGateway>(ResourceNameApi.GatewayBff)
                     .WithReference(identityProvider)
                     .WaitFor(identityProvider)
                     .WithReference(orderingApi)
                     .WaitFor(orderingApi)
                     .WithExternalHttpEndpoints();

identityProvider
    .WithEnvironment(EnvironmentNameService.Ordering, orderingApi.GetEndpoint(launchProfileName))
    .WithEnvironment(EnvironmentNameService.Gateway, gateway.GetEndpoint(launchProfileName));

gateway
    .WithEnvironment("BFF__Authority", identityEndpoint)
    .WithEnvironment(
        "BFF__Api__RemoteUrl",
        "http://localhost:5272/api");

await builder.Build().RunAsync();
