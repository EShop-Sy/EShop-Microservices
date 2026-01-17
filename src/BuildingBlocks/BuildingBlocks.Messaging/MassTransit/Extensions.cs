using MassTransit;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Messaging.MassTransit;

public static class Extensions
{
    public static IHostApplicationBuilder AddBroker(this IHostApplicationBuilder builder, Assembly? assembly = null)
    {
        builder.Services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            if (assembly != null) config.AddConsumers(assembly);

            config.UsingRabbitMq((context, cfg) =>
            {
                var connectionString = builder.Configuration.GetConnectionString("messaging");

                cfg.Host(connectionString);

                cfg.ConfigureEndpoints(context);
            });
        });

        return builder;
    }
}
