namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketCommand(BasketCheckoutDto Cart) : ICommand<CheckoutBasketResult>;

public record CheckoutBasketResult(Guid Id);

public class CheckoutBasketHandler(IBasketRepository repository, IPublishEndpoint publishEndpoint)
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasket(command.Cart.Id, cancellationToken);

        var data = command.Cart.Adapt<BasketCheckoutEventData>();

        data.Items = basket.Items.Adapt<List<Item>>();

        data.TotalPrice = basket.TotalPrice;

        var eventMessage = new BasketCheckoutEvent(data);

        await publishEndpoint.Publish(eventMessage, cancellationToken);

        await repository.DeleteBasket(command.Cart.Id, cancellationToken);

        return new CheckoutBasketResult(command.Cart.Id);
    }
}
