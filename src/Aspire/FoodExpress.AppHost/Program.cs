var builder = DistributedApplication.CreateBuilder(args);

builder.AddForwardedHeaders();

var postgresUserName = builder.AddParameter("postgres-username", true);
var postgresPassword = builder.AddParameter("postgres-password", true);

var keycloakUserName = builder.AddParameter("keycloak-username", true);
var keycloakPassword = builder.AddParameter("keycloak-password", true);

var launchProfileName = builder.Configuration["DOTNET_LAUNCH_PROFILE"] ?? "http";

// Keycloak resource
var identityProvider = builder.AddKeycloak(ServiceName.IdentityProvider, 18080, keycloakUserName, keycloakPassword)
                              .WithExternalHttpEndpoints()
                              .WithDataVolume();

// Redis resource
var redis = builder
            .AddRedis(ServiceName.Redis, 6379)
            .WithRedisInsight()
            .WithDataVolume(isReadOnly: false)
            .WithLifetime(ContainerLifetime.Persistent);

var postgres = builder
               .AddPostgres(ServiceName.Postgres, postgresUserName, postgresPassword, 15432)
               .WithDataVolume(isReadOnly: false)
               .WithLifetime(ContainerLifetime.Persistent);

var catalogDb = postgres.AddDatabase(ServiceName.Catalog);

if (builder.Environment.IsDevelopment())
{
    identityProvider.WithRealmImport("./Keycloak/");
}

var identityEndpoint = identityProvider.GetEndpoint(launchProfileName);

// catalog service api
var catalogApi = builder.AddProject<Services_Catalog_Api>(ResourceNameApi.Catalog)
                        .WithReference(identityProvider)
                        .WithReference(catalogDb)
                        .WaitFor(identityProvider)
                        .WaitFor(catalogDb)
                        .WithEnvironment(EnvironmentNameService.Identity, identityEndpoint);

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
    .WithEnvironment(EnvironmentNameService.Catalog, catalogApi.GetEndpoint(launchProfileName))
    .WithEnvironment(EnvironmentNameService.Ordering, orderingApi.GetEndpoint(launchProfileName))
    .WithEnvironment(EnvironmentNameService.Gateway, gateway.GetEndpoint(launchProfileName));

await builder.Build().RunAsync();
