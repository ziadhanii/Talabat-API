namespace Talabat.Repository.Data.Config;

internal class ProductBrandConfiguration : IEntityTypeConfiguration<ProductBrand>
{
    public void Configure(EntityTypeBuilder<ProductBrand> builder)
    {
        builder.Property(B => B.Name)
            .IsRequired();
    }
}