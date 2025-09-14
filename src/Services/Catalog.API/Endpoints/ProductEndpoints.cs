using Catalog.API.Models;
using Catalog.API.Services;

namespace Catalog.API.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products").WithGroupName("products").RequireAuthorization();

        // GET all
        group.MapGet("/", async (ProductService service) =>
            {
                var products = await service.GetProductsAsync();
                return products;
            })
            .WithName("GetAllProducts")
            .Produces<List<Product>>();

        // GET by ID
        group.MapGet("/{id:guid}", async (Guid id, ProductService service) =>
            {
                var product = await service.GetProductByIdAsync(id);

                return product is null ? Results.NotFound() : Results.Ok(product);
            })
            .WithName("GetProductById")
            .Produces<Product>()
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}