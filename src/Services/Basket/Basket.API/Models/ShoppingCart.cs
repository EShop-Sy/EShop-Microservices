namespace Basket.API.Models;

public class ShoppingCart
{
    public Guid Id { get; init; }

    public ICollection<ShoppingCartItem> Items { get; } = new List<ShoppingCartItem>();

    public decimal TotalPrice => Items.Sum(i => i.PriceAtAddition * i.Quantity);
}
