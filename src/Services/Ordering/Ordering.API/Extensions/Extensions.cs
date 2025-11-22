using System.Reflection;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Messaging.MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Application.Extensions;

public static class Extensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection svc, IConfiguration conf)
    {
        svc.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        svc.AddMessageBroker(conf, Assembly.GetExecutingAssembly());

        return svc;
    }

    public static IEnumerable<OrderDto> ToOrderDtoList(this IEnumerable<Order> orders)
    {
        return orders.Select(order => new OrderDto(
            order.Id.Value,
            order.CustomerId,
            order.ShippingAddress,
            order.Status,
            order.OrderItems.Select(oi => new OrderItemDto(oi.ProductId.Value, oi.Quantity, oi.Price)).ToList()
        ));
    }

    public static OrderDto ToOrderDto(this Order order)
    {
        return DtoFromOrder(order);
    }

    private static OrderDto DtoFromOrder(Order order)
    {
        return new OrderDto(
            order.Id.Value,
            order.CustomerId,
            order.ShippingAddress,
            order.Status,
            order.OrderItems.Select(oi => new OrderItemDto(oi.ProductId.Value, oi.Quantity, oi.Price)).ToList()
        );
    }
}
