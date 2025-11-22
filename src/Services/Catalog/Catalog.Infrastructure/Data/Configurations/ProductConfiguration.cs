namespace Catalog.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        #region Index

        builder.HasIndex(c => c.Name);

        builder.HasIndex(c => c.Price);

        builder.HasIndex(c => c.CreatedAt);

        builder.HasIndex(c => c.LastModified);

        #endregion

        #region Filter

        builder.HasQueryFilter(p => !p.IsDeleted);

        #endregion
    }
}
