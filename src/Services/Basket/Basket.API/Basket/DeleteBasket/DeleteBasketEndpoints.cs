namespace Basket.API.Basket.DeleteBasket;

public class DeleteBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{id:guid}", async (Guid id, ISender sender) =>
            {
                await sender.Send(new DeleteBasketCommand(id));

                return Results.NoContent();
            })
            .WithName("DeleteBasket")
            .WithSummary("Delete Basket")
            .WithDescription("Delete Basket")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
