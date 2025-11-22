namespace Catalog.API.Endpoints;

public record UpdateProductRequest(
    Guid Id,
    string? Name,
    string? Description,
    string? Image,
    decimal? Price,
    int? Stock);

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateProductCommand>();

                var result = await sender.Send(command);

                return Results.NoContent();
            })
            .WithName("UpdateProduct")
            .WithSummary("Update Product")
            .WithDescription("Update Product")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .DisableAntiforgery()
            .RequireAuthorization();
    }
}
