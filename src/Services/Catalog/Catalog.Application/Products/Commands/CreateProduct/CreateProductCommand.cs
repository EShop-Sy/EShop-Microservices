namespace Catalog.Application.Products.Commands.CreateProduct;

public record CreateProductCommand(string Name, string Description, IFormFile Image, decimal Price, int Stock)
    : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);
