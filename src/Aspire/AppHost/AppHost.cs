using Aspire.Hosting.Yarp.Transforms;

var builder = DistributedApplication.CreateBuilder(args);

// Backing Services
// var rg = builder.AddParameter("ResourceGroup", "rg-dev");

// Database
// var postgresName = builder.AddParameter("PostgresName", "postgres");

var postgres = builder.AddAzurePostgresFlexibleServer("postgres");

// if (builder.Environment.IsDevelopment())
// {
//     postgres.RunAsContainer();
// }
// else
// {
//     postgres.AsExisting(postgresName, rg);
// }

var catalogdb = postgres.AddDatabase("catalogdb");

// Key Vault
// var keyVaultName = builder.AddParameter("KeyVaultName", "keyvault-c3bqwnyvzo3w4");

var keyVault = builder.AddAzureKeyVault("key-vault");
// .AsExisting(keyVaultName, rg);

var apiKey = builder.AddParameter("ApiKeySecret", secret: true);

keyVault.AddSecret("ApiKey", apiKey);

// Projects
var migrations = builder.AddProject<Projects.MigrationService>("migrations")
    .WithReference(catalogdb)
    .WaitFor(catalogdb);
// .WithParentRelationship(catalogdb);

var catalog = builder.AddProject<Projects.Catalog_API>("catalog")
    .WithReference(keyVault)
    .WithReference(catalogdb)
    .WithReference(migrations)
    .WaitFor(keyVault)
    .WaitFor(catalogdb)
    .WaitForCompletion(migrations);

var basket = builder.AddProject<Projects.Basket_API>("basket")
    .WithReference(keyVault)
    .WaitFor(keyVault);

// Reverse Proxy
builder.AddYarp("api-gateway-mobile").WithConfiguration(yarp =>
    {
        var catalogcluster = yarp.AddCluster(catalog);

        yarp.AddRoute("/catalog/{**catch-all}", catalogcluster)
            .WithTransformPathRemovePrefix("/catalog")
            .WithTransformRequestHeader("X-Forwarded-Host", "gateway.eshop.sy.com")
            .WithTransformResponseHeader("X-Powered-By", "YARP");

        var basketcluster = yarp.AddCluster(basket);

        yarp.AddRoute("/basket/{**catch-all}", basketcluster)
            .WithTransformPathRemovePrefix("/basket")
            .WithTransformRequestHeader("X-Forwarded-Host", "gateway.eshop.sy.com")
            .WithTransformResponseHeader("X-Powered-By", "YARP");
    })
    .WithExternalHttpEndpoints();

await builder.Build().RunAsync();