using Catalog.Application.Products.Commands.DeleteProduct;

namespace Catalog.API.Endpoints;

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id:guid}", async (Guid id, ISender sender) =>
            {
                await sender.Send(new DeleteProductCommand(id));

                return Results.NoContent();
            })
            .WithName("DeleteProduct")
            .WithDescription("Delete Product By Id")
            .WithSummary("Delete Product By Id")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
