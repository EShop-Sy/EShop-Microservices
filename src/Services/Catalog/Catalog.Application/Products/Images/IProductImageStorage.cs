namespace Catalog.Application.Products.Images;

public interface IProductImageStorage
{
    Task<Uri> UploadAsync(IFormFile file, CancellationToken cancellationToken = default);
}
