var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.AddApplicationServices().AddInfrastructureServices().AddApiServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler(_ => { });

app.UseApiServices();

app.MapDefaultEndpoints();

await app.RunAsync();
