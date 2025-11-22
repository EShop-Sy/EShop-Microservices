namespace Catalog.Domain.Models;

public class Product(string name, string description, string imageUrl, decimal price, int stock)
{
    #region Properties

    public Guid Id { get; init; }

    [MaxLength(50)] public string Name { get; set; } = name;

    [MaxLength(100)] public string Description { get; set; } = description;

    [MaxLength(250)] public string ImageUrl { get; set; } = imageUrl;

    public decimal Price { get; set; } = price;

    public int Stock { get; set; } = stock;

    public bool IsDeleted { get; init; }

    #endregion

    #region Computed Properties

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; init; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime LastModified { get; init; }

    #endregion
}
