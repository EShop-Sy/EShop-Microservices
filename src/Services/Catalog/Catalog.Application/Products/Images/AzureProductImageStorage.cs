namespace Catalog.Application.Products.Images;

internal class AzureProductImageStorage(BlobServiceClient client) : IProductImageStorage
{
    private const string ContainerName = "products";

    public async Task<Uri> UploadAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);

        var container = client.GetBlobContainerClient(ContainerName);
        await container.UploadBlobAsync(file.FileName, file.OpenReadStream(), cancellationToken);
        return new Uri($"{container.Uri}/{file.FileName}");
    }
}
