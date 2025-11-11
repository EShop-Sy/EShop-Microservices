using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Authentication;

public static class Extensions
{
    public static IServiceCollection AddKeycloakAuthentication(this IServiceCollection services,
        IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddAuthentication().AddKeycloakJwtBearer(serviceName: "keycloak", realm: "eshop", options =>
        {
            options.Audience = "store.api";

            // Disable HTTPS metadata validation in development
            if (environment.IsDevelopment())
            {
                options.RequireHttpsMetadata = false;
            }
            else
            {
                // Set the Authority
                var connection = configuration.GetConnectionString("keycloak");
                options.Authority = $"{connection}/realms/eshop";
            }
        });

        services.AddAuthorizationBuilder();

        return services;
    }
}
