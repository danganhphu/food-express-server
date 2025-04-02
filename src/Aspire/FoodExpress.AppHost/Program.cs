using FoodExpress.ScalarOpenApi;

var builder = DistributedApplication.CreateBuilder(args);

var postgresUserName = builder.AddParameter("postgres-username", true);
var postgresPassword = builder.AddParameter("postgres-password", true);

var keycloakUserName = builder.AddParameter("keycloak-username", true);
var keycloakPassword = builder.AddParameter("keycloak-password", true);

var postgres = builder
               .AddPostgres(ServiceName.Postgres, postgresUserName, postgresPassword, 15432)
               .WithImageTag("17.2")
               .WithDataVolume(isReadOnly: false)
               .WithLifetime(ContainerLifetime.Persistent);

// Keycloak resource
var identityProvider = builder.AddKeycloak(ServiceName.IdentityProvider, 18080, keycloakUserName, keycloakPassword)
                              .WithImageTag("26.1")
                              .WithExternalHttpEndpoints()
                              .WithDataVolume();

// Redis resource
var redis = builder
            .AddRedis(ServiceName.Redis, 6379)
            .WithRedisInsight()
            .WithDataVolume(isReadOnly: false)
            .WithLifetime(ContainerLifetime.Persistent);

var qdrant = builder
             .AddQdrant(ServiceName.VectorDb)
             .WithDataVolume()
             .WithLifetime(ContainerLifetime.Persistent);

var queue = builder
            .AddRabbitMQ(ServiceName.Queue)
            .WithManagementPlugin()
            .WithLifetime(ContainerLifetime.Persistent)
            .WithEndpoint("tcp", e => e.Port = 5672)
            .WithEndpoint("management", e => e.Port = 15672);

var cosmos = builder.AddAzureCosmosDB(ServiceName.Cosmos);
var storage = builder.AddAzureStorage("storage");
var signalR = builder.AddAzureSignalR(ServiceName.SignalR);

builder.ConfigAzureResource(cosmos, storage, signalR);

var catalogDb = postgres.AddDatabase(ServiceName.Catalog);

if (builder.Environment.IsDevelopment())
{
    identityProvider.WithRealmImport("./Keycloak/");
}

var mailpit = builder.AddMailPit(ServiceName.MailPit);


// catalog service api
var catalogApi = builder.AddProject<Services_Catalog_Api>(ResourceNameApi.Catalog)
                        .WithScalar()
                        .WithReplicas(2)
                        .WithReference(identityProvider)
                        .WithReference(catalogDb)
                        .WithReference(redis)
                        .WaitFor(identityProvider)
                        .WaitFor(catalogDb)
                        .WaitFor(redis);

// ordering service api
var orderingApi = builder.AddProject<Services_Ordering_Api>(ResourceNameApi.Order)
                         .WithReference(identityProvider)
                         .WaitFor(identityProvider);

// api gateway - Yarp ReverseProxy
var gateway = builder.AddProject<FoodExpress_ApiGateway>(ResourceNameApi.GatewayBff)
                     .WithReference(identityProvider)
                     .WithReference(redis)
                     .WithReference(orderingApi)
                     .WaitFor(identityProvider)
                     .WaitFor(redis)
                     .WaitFor(orderingApi)
                     .WithExternalHttpEndpoints();


await builder.Build().RunAsync();
