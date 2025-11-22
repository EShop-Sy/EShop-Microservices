using Catalog.Application.Products.Images;

namespace Catalog.Application.Products.Commands.CreateProduct;

internal class CreateProductCommandHandler(ICatalogDbContext context, IProductImageStorage imageStorage)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var image = await imageStorage.UploadAsync(command.Image, cancellationToken);
        var product = new Product(command.Name, command.Description, image.AbsoluteUri, command.Price, command.Stock);

        await context.Products.AddAsync(product, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }
}
