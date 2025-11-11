using Microsoft.EntityFrameworkCore;

namespace Basket.API.Repository;

public class BasketRepository(BasketDbContext context) : IBasketRepository
{
    public async Task<ShoppingCart> GetBasket(Guid id, CancellationToken cancellationToken = default)
    {
        var basket = await context.ShoppingCarts.FindAsync([id], cancellationToken);

        return basket ?? throw new BasketNotFoundException(id);
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        await context.ShoppingCarts.AddAsync(basket, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return basket;
    }

    public async Task<bool> DeleteBasket(Guid id, CancellationToken cancellationToken = default)
    {
        var basket = await context.ShoppingCarts.SingleAsync(c => c.Id == id, cancellationToken);

        context.ShoppingCarts.Remove(basket);

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
