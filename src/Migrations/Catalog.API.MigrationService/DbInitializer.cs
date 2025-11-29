namespace Catalog.API.MigrationService;

public class DbInitializer(IServiceProvider svc, IHostApplicationLifetime applicationLifetime) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = s_activitySource.StartActivity(ActivityKind.Client);

        try
        {
            using var scope = svc.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

            await InitDatabaseAsync(dbContext, stoppingToken);

            applicationLifetime.StopApplication();
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }
    }

    private static async Task InitDatabaseAsync(CatalogDbContext dbContext, CancellationToken stoppingToken = default)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () => await dbContext.Database.MigrateAsync(stoppingToken));
    }
}
