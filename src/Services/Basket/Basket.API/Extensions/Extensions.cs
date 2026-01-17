namespace Basket.API.Extensions;

public static class Extensions
{
    public static TBuilder AddApiServices<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.AddKeycloak();

        builder.AddEndpoints();

        builder.AddCQRS();

        builder.AddDocumentDb();

        builder.AddDataServices();

        builder.AddRedisCache();

        builder.AddBroker();

        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        return builder;
    }

    private static TBuilder AddEndpoints<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddCarter();

        return builder;
    }

    private static TBuilder AddCQRS<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        var assembly = typeof(Program).Assembly;

        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);

            config.AddOpenBehavior(typeof(ValidationBehavior<,>));

            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        builder.Services.AddValidatorsFromAssembly(assembly);

        return builder;
    }

    private static TBuilder AddDocumentDb<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.AddNpgsqlDataSource("basketdb");

        builder.Services.AddMarten(opts =>
            {
                opts.DatabaseSchemaName = "basketdb";

                opts.AutoCreateSchemaObjects = AutoCreate.All;

                opts.Policies.ForAllDocuments(m =>
                {
                    if (m.IdType == typeof(Guid))
                    {
                        m.IdStrategy = new SequentialGuidIdGeneration();
                    }
                });
            })
            .UseLightweightSessions()
            .UseNpgsqlDataSource()
            .ApplyAllDatabaseChangesOnStartup();

        return builder;
    }

    private static TBuilder AddDataServices<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddScoped<IBasketRepository, BasketRepository>();

        builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

        return builder;
    }

    private static TBuilder AddRedisCache<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.AddRedisDistributedCache("cache");

        return builder;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.MapCarter();

        app.UseExceptionHandler(_ => { });

        app.MapDefaultEndpoints();

        return app;
    }
}
