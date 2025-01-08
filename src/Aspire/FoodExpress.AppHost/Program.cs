var builder = DistributedApplication.CreateBuilder(args);

var launchProfileName = builder.Configuration["DOTNET_LAUNCH_PROFILE"] ?? "http";

// Services
var identityProvider = builder.AddKeycloak(ServiceName.IdentityProvider, 8080)
                              .WithExternalHttpEndpoints()
                              .WithDataVolume();

if (builder.Environment.IsDevelopment())
{
    identityProvider.WithRealmImport("./Keycloak/");
}

_ = identityProvider.GetEndpoint(launchProfileName);


builder.AddProject<Projects.FoodExpress_ApiGateway>(ResourceNameApi.GatewayBff)
       .WithReference(identityProvider)
       .WaitFor(identityProvider)
       .WithExternalHttpEndpoints();

await builder.Build().RunAsync();
