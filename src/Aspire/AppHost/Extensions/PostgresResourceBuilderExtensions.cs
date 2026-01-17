using Aspire.Hosting.Azure;

namespace AppHost.Extensions;

internal static class PostgresResourceBuilderExtensions
{
    public static DatabaseResources WithDatabases(this IResourceBuilder<AzurePostgresFlexibleServerResource> builder)
    {
        return new DatabaseResources(builder.AddDatabase("basketdb"));
    }
}

public record DatabaseResources(
    IResourceBuilder<AzurePostgresFlexibleServerDatabaseResource> BasketDb
);
