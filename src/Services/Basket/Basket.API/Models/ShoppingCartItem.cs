namespace Basket.API.Models;

public class ShoppingCartItem
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal PriceAtAddition { get; set; }

    public Guid ShoppingCartId { get; set; } // Required foreign key property

    public ShoppingCart ShoppingCart { get; set; } = null!; // Required reference navigation to principal
}
