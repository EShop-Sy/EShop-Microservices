namespace Basket.API.Dtos;

public class BasketCheckoutDto
{
    public Guid Id { get; set; }

    // shipping address
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public required string AddressLine { get; set; }
}
