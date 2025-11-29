var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHostedService<DbInitializer>();

builder.Services.AddOpenTelemetry().WithTracing(tracing => tracing.AddSource(DbInitializer.ActivitySourceName));

builder.AddAzureNpgsqlDbContext<BasketDbContext>(connectionName: "basketdb");

var host = builder.Build();

await host.RunAsync();
