namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

public record StoreBasketResult(Guid Id);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart.Items.Count).NotEmpty().WithMessage("Cart items can not be empty");
    }
}

public class StoreBasketCommandHandler(IBasketRepository repository)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        await repository.StoreBasket(command.Cart, cancellationToken);

        return new StoreBasketResult(command.Cart.Id);
    }
}
