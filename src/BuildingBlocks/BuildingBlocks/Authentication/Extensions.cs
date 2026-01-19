using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Authentication;

public static class Extensions
{
    public static TBuilder AddKeycloak<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddAuthentication().AddKeycloakJwtBearer(serviceName: "keycloak-auth-service", realm: "eshop",
            options =>
            {
                options.Audience = "store.api";

                // Explicitly set the Authority for production
                if (!builder.Environment.IsDevelopment())
                {
                    options.Authority = "https://keycloak-service.internal/realms/eshop";
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
