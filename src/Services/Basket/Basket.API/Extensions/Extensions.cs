namespace Basket.API.Extensions;

public static class Extensions
{
    public static TBuilder AddApiServices<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        // Add Authentication Services
        builder.Services.AddKeycloakAuthentication(builder.Configuration, builder.Environment);

        // Add Database Services
        builder.AddAzureNpgsqlDbContext<BasketDbContext>("basketdb");

        // Add Cache Services
        builder.AddRedisDistributedCache("cache");

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

        // Add Exception Handling
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        return builder;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.MapCarter();

        return app;
    }
}
