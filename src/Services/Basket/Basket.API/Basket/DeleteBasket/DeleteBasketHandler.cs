namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketCommand(Guid Id) : ICommand<DeleteBasketResult>;

public record DeleteBasketResult(bool IsSuccess);

public class DeleteBasketHandler(IBasketRepository repository)
    : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        await repository.DeleteBasket(command.Id, cancellationToken);

        return new DeleteBasketResult(true);
    }
}
