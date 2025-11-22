namespace Catalog.Application.Products.EventHandlers.Integration;

public class BasketCheckoutEventHandler(ISender sender, ILogger<BasketCheckoutEventHandler> logger)
    : IConsumer<BasketCheckoutEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);
        }

        foreach (var item in context.Message.Items)
        {
            var command = MapToDecrementProductStockCommand(item);

            await sender.Send(command);
        }
    }

    private static DecrementProductStockCommand MapToDecrementProductStockCommand(Item product)
    {
        return new DecrementProductStockCommand(product.ProductId, product.Quantity);
    }
}
