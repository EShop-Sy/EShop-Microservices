using Basket.API.Repository;

namespace Basket.API.Extensions;

public static class Extensions
{
    public static TBuilder AddApiServices<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        // Add Authentication Services
        builder.Services.AddKeycloakAuthentication(builder.Environment);

        // Add Database Services
        builder.AddAzureNpgsqlDbContext<BasketDbContext>(connectionName: "basketdb");

        // Add Cache Services
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("cache");
        });

        // Async Communication Services
        builder.Services.AddMessageBroker(builder.Configuration);

        // Add API Services
        builder.Services.AddCarter();

        // Add MediatR Services
        var assembly = typeof(Program).Assembly;

        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        // Add Repositories
        builder.Services.AddScoped<IBasketRepository, BasketRepository>();

        builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

        return builder;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.MapCarter();

        return app;
    }
}
