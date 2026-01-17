using Aspire.Hosting.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace AppHost.Extensions;

internal static class RedisResourceBuilderExtensions
{
    public static IResourceBuilder<AzureManagedRedisResource> WithClearCommand(
        this IResourceBuilder<AzureManagedRedisResource> builder)
    {
        var commandOptions = new CommandOptions
        {
            UpdateState = OnUpdateResourceState, IconName = "AnimalRabbitOff", IconVariant = IconVariant.Filled
        };

        builder.WithCommand(
            name: "clear-cache",
            displayName: "Clear Cache",
            executeCommand: _ => OnRunClearCacheCommandAsync(builder),
            commandOptions: commandOptions);

        return builder;
    }

    private static async Task<ExecuteCommandResult> OnRunClearCacheCommandAsync(
        IResourceBuilder<AzureManagedRedisResource> builder)
    {
        var connectionString =
            await builder.Resource.ConnectionStringExpression.GetValueAsync(CancellationToken.None) ??
            throw new InvalidOperationException("Unable to get the '{context.ResourceName}' connection string.");

        await using var connection = await ConnectionMultiplexer.ConnectAsync(connectionString);
        var database = connection.GetDatabase();
        await database.ExecuteAsync("FLUSHALL");

        return CommandResults.Success();
    }

    private static ResourceCommandState OnUpdateResourceState(UpdateCommandStateContext context)
    {
        var logger = context.ServiceProvider.GetRequiredService<ILogger<Program>>();
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Updating resource state: {ResourceSnapshot}", context.ResourceSnapshot);
        }

        return context.ResourceSnapshot.HealthStatus is HealthStatus.Healthy
            ? ResourceCommandState.Enabled
            : ResourceCommandState.Disabled;
    }
}
