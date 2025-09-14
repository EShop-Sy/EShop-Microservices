namespace Catalog.API.Models;

/// <summary>
/// Product model
/// </summary>
public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public List<string> Category { get; set; } = [];
    public string Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}