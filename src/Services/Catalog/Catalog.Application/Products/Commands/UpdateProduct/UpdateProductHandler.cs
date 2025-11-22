using Catalog.Application.Products.Images;

namespace Catalog.Application.Products.Commands.UpdateProduct;

internal class UpdateProductCommandHandler(ICatalogDbContext context, IProductImageStorage imageStorage)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle([FromForm] UpdateProductCommand command, CancellationToken ct)
    {
        var product = await context.Products.SingleAsync(product => product.Id == command.Id, ct);

        if (product is null) throw new ProductNotFoundException(command.Id);

        if (command.Name != null) product.Name = command.Name;

        if (command.Description != null) product.Description = command.Description;

        if (command.Price != null) product.Price = command.Price.Value;

        if (command.Stock != null) product.Price = command.Stock.Value;

        if (command.Image != null)
        {
            var image = await imageStorage.UploadAsync(command.Image, ct);
            product.ImageUrl = image.AbsoluteUri;
        }

        await context.SaveChangesAsync(ct);

        return new UpdateProductResult();
    }
}
