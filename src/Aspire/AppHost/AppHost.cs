using AppHost.Extensions;

// using Azure.Provisioning.AppContainers;

var builder = DistributedApplication.CreateBuilder(args);

// Backing Services (Redis, RabbitMQ, etc.)
// Cache
// var cache = builder.AddAzureRedis("cache")
//     .RunAsContainer(resourceBuilder => resourceBuilder.WithContainerName("eshop-cache"))
//     .WithIconName("StackFilled");
// var acaEnv = builder.AddAzureContainerAppEnvironment("aca-env")
//      .WithAzdResourceNaming();

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
// var secret = builder.AddParameter("KeycloakClientSecret", secret: true);
// var keycloak = builder.AddKeycloak("keycloak-service", 8080)
//     .WithRealmImport("./realms")
//     .WithArgs("--http-enabled=true")
//     .WithArgs("--proxy-headers=forwarded")
//     // .WithArgs("--hostname=https://keycloak-service.internal.jollyground-0d1a883d.uaenorth.azurecontainerapps.io")
//     // .WithArgs("--hostname=keycloak-service.internal")
//     // .WithArgs("--hostname-debug=true")
//     // .WithArgs("--hostname-strict=false")
//     // .WithArgs("--hostname-backchannel-dynamic=false")
//     .WithArgs("--hostname=https://keycloak-service.internal.jollyground-0d1a883d.uaenorth.azurecontainerapps.io")
//     .WithEnvironment("CLIENT_SECRET", secret)
//     .WithIconName("KeyMultiple");

// builder.AddAzureContainerAppEnvironment()
var secret = builder.AddParameter("KeycloakClientSecret", secret: true);
var keycloak = builder.AddKeycloak("keycloak-service")
    .WithRealmImport("./realms")
    .WithArgs("--http-enabled=true")
    .WithArgs("--proxy-headers=forwarded")
    .WithArgs("--hostname=https://keycloak-service.internal.{{ .Env.AZURE_CONTAINER_APPS_ENVIRONMENT_DEFAULT_DOMAIN }}")
    .WithEnvironment("CLIENT_SECRET", secret)
    .WithIconName("KeyMultiple");

// var containerEnvironment = acaEnv.GetOutput("AZURE_CONTAINER_APPS_ENVIRONMENT_DEFAULT_DOMAIN").ValueExpression;


// var acaId = acaEnv.GetOutput("AZURE_CONTAINER_APPS_ENVIRONMENT_ID");
// var domainValue = acaEnv.GetOutput("AZURE_CONTAINER_APPS_ENVIRONMENT_DEFAULT_DOMAIN");
// var domainValue = acaEnv.Resource.NameOutputReference.Value;
// builder.Resources
// acaEnv.Resource.
// var t = builder.Configuration["AZURE_CONTAINER_APPS_ENVIRONMENT_DEFAULT_DOMAIN"];
// var tt = acaEnv.Resource.Outputs.TryGetValue("AZURE_CONTAINER_APPS_ENVIRONMENT_ID", out string domainValue);
// var domainValue = acaEnv.GetOutput("AZURE_CONTAINER_APPS_ENVIRONMENT_DEFAULT_DOMAIN").Value;
//
// if (builder.Configuration["AZURE_CONTAINER_APPS_ENVIRONMENT_DEFAULT_DOMAIN"] is { } domain)
// {
// keycloak.WithArgs($"--hostname=https://keycloak-service.internal.{containerEnvironment}");
// }

// keycloak
//     .WithEnvironment("CLIENT_SECRET", secret)
//     .WithIconName("KeyMultiple");

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
