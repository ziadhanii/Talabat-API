namespace Talabat.Repository.Data.Config;

public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.Property(C => C.Name)
            .IsRequired();
    }
}