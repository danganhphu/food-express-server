using Iass.SharedKernel.EFCore.Migrations;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.SharedKernel.EFCore.Migrations;

public static class MigrateDbContextExtensions
{
    private const string ActivitySourceName = "DbMigrations";
    private static readonly ActivitySource _activitySource = new(ActivitySourceName);

    public static IServiceCollection AddMigration<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        return services.AddMigration<TContext>((_, _) => Task.CompletedTask);
    }

    public static IServiceCollection AddMigration<TContext>(this IServiceCollection services,
                                                            Func<TContext, IServiceProvider, Task> seeder)
        where TContext : DbContext
    {
        services.AddOpenTelemetry().WithTracing(tracing => tracing.AddSource(ActivitySourceName));

        return services.AddHostedService(sp => new MigrationHostedService<TContext>(sp, seeder));
    }

    public static IServiceCollection AddMigration<TContext, TDbSeeder>(this IServiceCollection services)
        where TContext : DbContext
        where TDbSeeder : class, IDbSeeder<TContext>
    {
        services.AddScoped<IDbSeeder<TContext>, TDbSeeder>();

        return services.AddMigration<TContext>(
            (context, sp) => sp.GetRequiredService<IDbSeeder<TContext>>().SeedAsync(context));
    }

    private static async Task MigrateDbContextAsync<TContext>(this IServiceProvider services,
                                                              Func<TContext, IServiceProvider, Task> seeder)
        where TContext : DbContext
    {
        using var scope = services.CreateScope();
        var scopeServices = scope.ServiceProvider;
        var logger = scopeServices.GetRequiredService<ILogger<TContext>>();
        var context = scopeServices.GetService<TContext>();

        using var activity = _activitySource.StartActivity($"Migration operation {typeof(TContext).Name}");

        try
        {
            logger.LogInformation(
                "Migrating database associated with context {DbContextName}",
                typeof(TContext).Name);

            var strategy = context?.Database.CreateExecutionStrategy();

            if (strategy is not null)
            {
                await strategy.ExecuteAsync(() => InvokeSeeder(seeder!, context, scopeServices));
            }
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "An error occurred while migrating the database used on context {DbContextName}",
                typeof(TContext).Name);

            activity.SetExceptionTags(ex);

            throw;
        }
    }

    private static async Task InvokeSeeder<TContext>(Func<TContext, IServiceProvider, Task> seeder,
                                                     TContext context,
                                                     IServiceProvider services)
        where TContext : DbContext?
    {
        using var activity = _activitySource.StartActivity($"Migrating {typeof(TContext).Name}");

        try
        {
            await context?.Database.MigrateAsync()!;
            await seeder(context, services);
        }
        catch (Exception ex)
        {
            activity.SetExceptionTags(ex);

            throw;
        }
    }

    /// <summary>
    /// ref: // Here we use `IHostedService` instead of `BackgroundService` because:
    /// we want to have control for running async task in StartAsync method and wait for completion not running it in background like `BackgroundService` in its StartAsync
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="seeder"></param>
    /// <typeparam name="TContext"></typeparam>
    private class MigrationHostedService<TContext>(IServiceProvider serviceProvider,
                                                   Func<TContext, IServiceProvider, Task> seeder) : IHostedService
        where TContext : DbContext
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await serviceProvider.MigrateDbContextAsync(seeder);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

public interface IDbSeeder<in TContext> where TContext : DbContext
{
    Task SeedAsync(TContext context);
}
