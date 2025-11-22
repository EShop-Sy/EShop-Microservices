using BuildingBlocks.Authentication;
using BuildingBlocks.Exceptions.Handler;

namespace Catalog.API.Extensions;

public static class Extensions
{
    public static TBuilder AddApiServices<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        // Add Authentication Services
        builder.Services.AddKeycloakAuthentication(builder.Configuration, builder.Environment);

        // Add Antiforgery Services
        builder.Services.AddAntiforgery();

        // Add API Services
        builder.Services.AddCarter();

        // Add Exception Handling
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        return builder;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.MapCarter();

        app.UseAntiforgery();

        return app;
    }
}
