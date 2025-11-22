namespace Catalog.Application.Products.Commands.DecrementProductStock;

public record DecrementProductStockCommand(Guid Id, int Stock) : ICommand<DecrementProductStockResult>;

public record DecrementProductStockResult;
