namespace Catalog.Application.Exceptions;

public class ProductNotFoundException(Guid id) : NotFoundException("Product", id);
