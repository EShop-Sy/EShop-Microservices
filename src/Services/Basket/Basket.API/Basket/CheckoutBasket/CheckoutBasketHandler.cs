using Basket.API.Repository;

namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto) : ICommand<CheckoutBasketResult>;

public record CheckoutBasketResult;

public class CheckoutBasketHandler(IBasketRepository repository, IPublishEndpoint publishEndpoint)
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasket(command.BasketCheckoutDto.Id, cancellationToken);

        var eventMessage = command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();

        eventMessage.TotalPrice = basket.TotalPrice;

        eventMessage.Items = basket.Items.Adapt<List<Item>>();

        await publishEndpoint.Publish(eventMessage, cancellationToken);

        await repository.DeleteBasket(command.BasketCheckoutDto.Id, cancellationToken);

        return new CheckoutBasketResult();
    }
}
