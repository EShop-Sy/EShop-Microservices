namespace BuildingBlocks.Messaging.Events;

public record Item(Guid ProductId, int Quantity, decimal Price);

public record BasketCheckoutEventData
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string PhoneNumber { get; set; }

    public required string AddressLine { get; set; }

    public required IReadOnlyList<Item> Items { get; set; }

    public required decimal TotalPrice { get; set; }
}

public record BasketCheckoutEvent(BasketCheckoutEventData Data) : IntegrationEvent;
