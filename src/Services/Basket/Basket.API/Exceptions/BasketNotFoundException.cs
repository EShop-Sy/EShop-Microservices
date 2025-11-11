namespace Basket.API.Exceptions;

public class BasketNotFoundException(Guid id) : NotFoundException("Basket", id);
