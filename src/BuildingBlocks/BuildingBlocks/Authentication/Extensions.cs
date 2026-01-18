using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Authentication;

// Add Authentication Services
public static class Extensions
{
    public static TBuilder AddKeycloak<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder

    {
        builder.Services.AddAuthentication().AddKeycloakJwtBearer(serviceName: "keycloak-auth-service", realm: "eshop", options =>
        {
            options.Audience = "store.api";

            // Explicitly set the Authority for production
            if (builder.Environment.IsProduction())
            {
                var url = builder.Configuration["KEYCLOAK_AUTH_SERVICE_HTTPS"];
                options.Authority = $"{url}/realms/eshop";
            }

            // For development only - disable HTTPS metadata validation
            // In production, use explicit Authority configuration instead
            if (builder.Environment.IsDevelopment())
            {
                options.RequireHttpsMetadata = false;
            }
        });

        builder.Services.AddAuthorizationBuilder();

        return builder;
    }
}
