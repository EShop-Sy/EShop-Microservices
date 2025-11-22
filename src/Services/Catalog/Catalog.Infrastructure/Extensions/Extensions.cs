namespace Catalog.Infrastructure.Extensions;

public static class Extensions
{
    public static TBuilder AddInfrastructureServices<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        // Add Database Services
        builder.AddAzureNpgsqlDbContext<CatalogDbContext>("catalogdb");

        builder.Services.AddScoped<ICatalogDbContext, CatalogDbContext>();

        return builder;
    }
}
