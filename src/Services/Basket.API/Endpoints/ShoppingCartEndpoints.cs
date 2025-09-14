using Basket.API.Models;
using Basket.API.Services;

namespace Basket.API.Endpoints;

public static class ShoppingCartEndpoints
{
    public static void MapBasketEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("cart").WithGroupName("cart").RequireAuthorization();

        // GET by id
        group.MapGet("/{id:guid}", async (Guid id, BasketService service) =>
            {
                var cart = await service.GetBasket(id);
                return cart is null ? Results.NotFound() : Results.Ok(cart);
            })
            .WithName("GetCartById")
            .Produces<ShoppingCart>()
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}