namespace Catalog.Application.Products.Queries.GetProducts;

internal class GetProductsQueryHandler(ICatalogDbContext context) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var reference = query.PaginationRequest.Reference;
        var size = query.PaginationRequest.PageSize;
        var count = await context.Products.LongCountAsync(cancellationToken);

        var products = await context.Products
            .OrderBy(product => product.CreatedAt)
            .ThenBy(product => product.Id)
            .Where(product => product.Id > reference)
            .Take(size)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new GetProductsResult(new PaginatedResult<Product>(products.Last().Id, size, count, products));
    }
}
