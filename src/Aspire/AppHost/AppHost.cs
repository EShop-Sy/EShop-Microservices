using AppHost.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

// Backing Services (Redis, RabbitMQ, etc.)
// Cache
var cache = builder.AddAzureRedis("cache")
    .RunAsContainer(resourceBuilder => resourceBuilder.WithContainerName("eshop-cache"))
    .WithIconName("StackFilled");

// Databases
var databases = builder.AddAzurePostgresFlexibleServer("postgres")
    .RunAsContainer(resourceBuilder => resourceBuilder.WithContainerName("eshop-postgres"))
    .WithDatabases();

// RabbitMQ
var username = builder.AddParameter("RabbitMQUserName", secret: true);
var password = builder.AddParameter("RabbitMQPassword", secret: true);
var rabbitmq = builder
    .AddRabbitMQ("messaging", username, password, 5672)
    .WithContainerName("eshop-rabbitmq")
    .WithManagementPlugin()
    .WithIconName("Connected");

// Authentication
var secret = builder.AddParameter("KeycloakClientSecret", secret: true);

var keycloak = builder.AddKeycloak("keycloak-auth-service")
    .WithRealmImport("./realms")
    .WithEnvironment("CLIENT_SECRET", secret)
    .WithArgs("--http-enabled=true")
    .WithArgs("--hostname=localhost")
    .WithOtlpExporter()
    .WithHttpsEndpoint()
    .WithIconName("KeyMultiple");

// #pragma warning disable ASPIRECERTIFICATES001
// var keycloak = builder.AddKeycloak("keycloak")
//     .WithRealmImport("./realms")
//     .WithEnvironment("CLIENT_SECRET", secret)
//     .WithEnabledFeatures()
//     .WithHttpsDeveloperCertificate()
//     .WithDeveloperCertificateTrust(true)
//     .WithOtlpExporter()
//     .WithIconName("KeyMultiple");
// #pragma warning restore ASPIRECERTIFICATES001

// Projects
var basket = builder.AddProject<Projects.Basket_API>("basket-service")
    .WithReferences([keycloak])
    .WithReferences([databases.BasketDb, cache, rabbitmq])
    .WithHttpHealthCheck("/health")
    .WithIconName("Cart");

// API Gateway
builder.AddYarp("api-gateway-mobile")
    .WithSettings(
        new Dictionary<string, IResourceBuilder<IResourceWithServiceDiscovery>>
        {
            { "basket", basket }, { "keycloak", keycloak }
        })
    .WaitForStart(keycloak)
    .WaitForStart(basket)
    .WithIconName("ArrowSplit")
    .WithExternalHttpEndpoints();

await builder.Build().RunAsync();
