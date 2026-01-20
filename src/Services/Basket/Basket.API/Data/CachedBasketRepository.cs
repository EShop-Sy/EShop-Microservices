namespace Basket.API.Data;

public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
{
    public async Task<ShoppingCart> GetBasket(Guid id, CancellationToken cancellationToken = default)
    {
        var cachedBasket = await cache.GetStringAsync(id.ToString(), cancellationToken);

        if (!string.IsNullOrEmpty(cachedBasket)) return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

        var basket = await repository.GetBasket(id, cancellationToken);

        await cache.SetStringAsync(id.ToString(), JsonSerializer.Serialize(basket), cancellationToken);

        return basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        await repository.StoreBasket(basket, cancellationToken);

        await cache.SetStringAsync(basket.Id.ToString(), JsonSerializer.Serialize(basket), cancellationToken);

        return basket;
    }

    public async Task<bool> DeleteBasket(Guid id, CancellationToken cancellationToken = default)
    {
        await repository.DeleteBasket(id, cancellationToken);

        await cache.RemoveAsync(id.ToString(), cancellationToken);

        return true;
    }
}
