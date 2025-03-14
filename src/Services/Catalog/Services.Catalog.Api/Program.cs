StaticLogger.EnsureInitialized();
Log.Information("server booting up..");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddServiceDefaults();

    builder.AddApplicationServices();

    var app = builder.Build();

    app.MapDefaultEndpoints();

    app.UseApplicationServices();

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
