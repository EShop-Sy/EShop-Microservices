using Basket.API.Models;

namespace Basket.API.Services;

public class BasketService
{
    public async Task<ShoppingCart?> GetBasket(Guid id)
    {
        var basket = new ShoppingCart(Guid.NewGuid(), []);
        return basket;
    }
}