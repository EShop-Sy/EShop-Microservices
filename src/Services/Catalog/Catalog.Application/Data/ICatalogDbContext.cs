namespace Catalog.Application.Data;

public interface ICatalogDbContext
{
    DbSet<Product> Products { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
