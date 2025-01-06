namespace BuildingBlocks.SharedKernel.Logging.Serilog;

public static class StaticLogger
{
    public static void EnsureInitialized()
    {
        if (Log.Logger is not Logger)
        {
        #pragma warning disable CA1305
            Log.Logger = new LoggerConfiguration()
                         .Enrich.FromLogContext()
                         .WriteTo.Console()
                     #pragma warning restore CA1305
                         .CreateLogger();
        }
    }
}
