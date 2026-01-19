using AppHost.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

// Backing Services (Redis, RabbitMQ, etc.)
// Cache
// var cache = builder.AddAzureRedis("cache")
//     .RunAsContainer(resourceBuilder => resourceBuilder.WithContainerName("eshop-cache"))
//     .WithIconName("StackFilled");

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
var keycloak = builder.AddKeycloak("keycloak-service", 8080)
    .WithRealmImport("./realms")
    .WithArgs("--http-enabled=true")
    .WithArgs("--proxy-headers=forwarded")
    .WithArgs("--hostname-strict=false")
    .WithArgs("--hostname-backchannel-dynamic=false")
    // .WithArgs("--hostname=https://keycloak-service.internal.jollyground-0d1a883d.uaenorth.azurecontainerapps.io")
    .WithEnvironment("CLIENT_SECRET", secret)
    .WithIconName("KeyMultiple");

// Projects
var basket = builder.AddProject<Projects.Basket_API>("basket-service")
    .WithReferences([keycloak])
    .WithReferences([databases.BasketDb, rabbitmq])
    // .WithReferences([databases.BasketDb, cache, rabbitmq])
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
