namespace Basket.API.Models;

public class ShoppingCart
{
    public Guid Id { get; init; }

    #region Navigation Properties

    public Guid CustomerId { get; init; }

    public ICollection<ShoppingCartItem> Items { get; init; } = new List<ShoppingCartItem>();

    #endregion

    #region Computed Properties

    public decimal TotalPrice => Items.Sum(i => i.Price * i.Quantity);

    #endregion
}
