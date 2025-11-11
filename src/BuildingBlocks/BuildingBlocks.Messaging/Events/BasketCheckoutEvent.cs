namespace BuildingBlocks.Messaging.Events;

public record Item(Guid ProductId, int Quantity, decimal PriceAtAddition);

public record BasketCheckoutEvent : IntegrationEvent
{
    // cart items
    public required IReadOnlyList<Item> Items { get; set; }

    public required decimal TotalPrice { get; set; }

    // shipping address
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string PhoneNumber { get; set; }

    public required string AddressLine { get; set; }
}
