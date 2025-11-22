using Catalog.Application.Products.Images;

namespace Catalog.Application.Extensions;

public static class Extensions
{
    public static TBuilder AddApplicationServices<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        // Async Communication Services
        builder.Services.AddMessageBroker(builder.Configuration, Assembly.GetExecutingAssembly());

        // Add Blob Storage Services
        builder.AddAzureBlobServiceClient("blobs");

        // Image Storage Services
        builder.Services.AddScoped<IProductImageStorage, AzureProductImageStorage>();

        // Add MediatR Services
        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        return builder;
    }
}
