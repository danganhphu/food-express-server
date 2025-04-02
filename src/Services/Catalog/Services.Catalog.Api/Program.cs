using Services.Catalog.Api;

StaticLogger.EnsureInitialized();
Log.Information("server booting up..");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddServiceDefaults();

    builder.AddApplicationServices();
    builder.Services.AddAsyncApiDocs([typeof(ICatalogApiAssemblyMaker)], "Catalog");

    var app = builder.Build();

    app.MapDefaultEndpoints();

    app.UseApplicationServices();

    app.MapAsyncApi();

    await app.RunAsync();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("HostAbortedException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
#pragma warning disable S6667
    Log.Fatal(ex.Message, "unhandled exception");
#pragma warning restore S6667
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("server shutting down..");
    await Log.CloseAndFlushAsync();
}
