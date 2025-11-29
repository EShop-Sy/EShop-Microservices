namespace Basket.API.Models;

public class ShoppingCartItem
{
    public Guid Id { get; set; }

    public int Quantity { get; set; }

    public decimal PriceAtAddition { get; set; }

    #region Navigation Properties

    public Guid ShoppingCartId { get; set; }

    public ShoppingCart ShoppingCart { get; set; } = null!;

    public Guid ProductId { get; set; }

    #endregion
}
