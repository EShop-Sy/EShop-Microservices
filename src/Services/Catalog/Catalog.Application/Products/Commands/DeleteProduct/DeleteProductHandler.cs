namespace Catalog.Application.Products.Commands.DeleteProduct;

internal class DeleteProductCommandHandler(ICatalogDbContext context)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        await context.Products.Where(p => p.Id == command.Id).ExecuteDeleteAsync(cancellationToken);

        return new DeleteProductResult();
    }
}
