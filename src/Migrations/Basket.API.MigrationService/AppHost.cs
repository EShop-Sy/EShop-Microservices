using Basket.API.Data;
using Basket.API.MigrationService;
using ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHostedService<BasketDbInitializer>();

builder.Services.AddOpenTelemetry().WithTracing(tracing => tracing.AddSource(BasketDbInitializer.ActivitySourceName));

builder.AddAzureNpgsqlDbContext<BasketDbContext>(connectionName: "basketdb");

var host = builder.Build();

await host.RunAsync();
