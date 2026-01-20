var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.AddApiServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseApiServices();

await app.RunAsync();
