namespace Catalog.Application.Products.Commands.DecrementProductStock;

internal class DecrementProductStockCommandHandler(ICatalogDbContext context)
    : ICommandHandler<DecrementProductStockCommand, DecrementProductStockResult>
{
    public async Task<DecrementProductStockResult> Handle(DecrementProductStockCommand command, CancellationToken ct)
    {
        var product = await context.Products.SingleAsync(p => p.Id == command.Id, ct);

        if (product is null)
        {
            throw new ProductNotFoundException(command.Id);
        }

        var stock = product.Stock - command.Stock;

        if (stock == 0)
        {
            context.Products.Remove(product);
        }
        else
        {
            product.Stock -= stock;
            await context.SaveChangesAsync(ct);
        }

        return new DecrementProductStockResult();
    }
}
