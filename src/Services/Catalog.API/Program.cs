using BuildingBlocks.Authentication;
using Catalog.API.Data;
using Catalog.API.Endpoints;
using Catalog.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();

builder.Configuration.AddAzureKeyVaultSecrets(connectionName: "key-vault");

var apiKey = builder.Configuration.GetValue<string>("ApiKey");

const string scheme = ApiKeyAuthenticationOptions.DefaultScheme;

builder.Services
    .AddAuthentication(scheme)
    .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(scheme, o => { o.ApiKey = apiKey; });

builder.Services.AddAuthorization();

builder.AddAzureNpgsqlDbContext<ProductDbContext>(connectionName: "catalogdb");

builder.Services.AddScoped<ProductService>();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthentication();

app.UseAuthorization();

app.MapDefaultEndpoints();

app.MapProductEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

await app.RunAsync();