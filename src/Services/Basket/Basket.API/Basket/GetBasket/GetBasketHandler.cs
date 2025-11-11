using Basket.API.Repository;

namespace Basket.API.Basket.GetBasket;

public record GetBasketQuery(Guid Id) : IQuery<GetBasketResult>;

public record GetBasketResult(ShoppingCart Cart);

public class GetBasketHandler(IBasketRepository repository) : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasket(query.Id, cancellationToken);

        return new GetBasketResult(basket);
    }
}
