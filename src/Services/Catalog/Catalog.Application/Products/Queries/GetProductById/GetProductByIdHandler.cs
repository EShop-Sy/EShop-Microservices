namespace Catalog.Application.Products.Queries.GetProductById;

internal class GetProductByIdQueryHandler(ICatalogDbContext context)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await context.Products.FindAsync([query.Id], cancellationToken: cancellationToken);

        return product is null ? throw new ProductNotFoundException(query.Id) : new GetProductByIdResult(product);
    }
}
