using Catalog.API.Data;
using Catalog.API.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry().WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddAzureNpgsqlDbContext<ProductDbContext>(connectionName: "catalogdb");

var host = builder.Build();

await host.RunAsync();