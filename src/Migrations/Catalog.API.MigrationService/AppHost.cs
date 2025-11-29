var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHostedService<DbInitializer>();

builder.Services.AddOpenTelemetry().WithTracing(tracing => tracing.AddSource(DbInitializer.ActivitySourceName));

builder.AddAzureNpgsqlDbContext<CatalogDbContext>(connectionName: "catalogdb");

builder.Services.AddScoped<ICatalogDbContext, CatalogDbContext>();

var host = builder.Build();

await host.RunAsync();
