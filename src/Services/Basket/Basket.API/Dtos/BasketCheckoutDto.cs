namespace Basket.API.Dtos;

public class BasketCheckoutDto
{
    public Guid Id { get; init; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string AddressLine { get; set; } = null!;
}
