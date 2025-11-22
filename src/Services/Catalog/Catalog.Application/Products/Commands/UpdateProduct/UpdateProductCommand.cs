namespace Catalog.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string? Name,
    string? Description,
    IFormFile? Image,
    decimal? Price,
    int? Stock) : ICommand<UpdateProductResult>;

public record UpdateProductResult;
