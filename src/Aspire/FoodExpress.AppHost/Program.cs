var builder = DistributedApplication.CreateBuilder(args);

builder.AddForwardedHeaders();

var launchProfileName = builder.Configuration["DOTNET_LAUNCH_PROFILE"] ?? "http";

// Keycloak resource
var identityProvider = builder.AddKeycloak(ServiceName.IdentityProvider, 18080)
                              .WithExternalHttpEndpoints()
                              .WithDataVolume();

// Redis resource
var redis = builder
            .AddRedis(ServiceName.Redis, 6379)
            .WithRedisInsight()
            .WithDataVolume(isReadOnly: false)
            .WithLifetime(ContainerLifetime.Persistent);

if (builder.Environment.IsDevelopment())
{
    identityProvider.WithRealmImport("./Keycloak/");
}

var identityEndpoint = identityProvider.GetEndpoint(launchProfileName);

// ordering service api
var orderingApi = builder.AddProject<Services_Ordering_Api>(ResourceNameApi.Order)
                         .WithReference(identityProvider)
                         .WaitFor(identityProvider)
                         .WithEnvironment(EnvironmentNameService.Identity, identityEndpoint);

// api gateway - Yarp ReverseProxy
var gateway = builder.AddProject<FoodExpress_ApiGateway>(ResourceNameApi.GatewayBff)
                     .WithReference(identityProvider)
                     .WithReference(redis)
                     .WithReference(orderingApi)
                     .WaitFor(identityProvider)
                     .WaitFor(redis)
                     .WaitFor(orderingApi)
                     .WithExternalHttpEndpoints();

identityProvider
    .WithEnvironment(EnvironmentNameService.Ordering, orderingApi.GetEndpoint(launchProfileName))
    .WithEnvironment(EnvironmentNameService.Gateway, gateway.GetEndpoint(launchProfileName));

//
// gateway
//     .WithEnvironment("BFF__Authority", identityEndpoint)
//     .WithEnvironment(
//         "BFF__Api__RemoteUrl",
//         "http://localhost:5272/api");

await builder.Build().RunAsync();
