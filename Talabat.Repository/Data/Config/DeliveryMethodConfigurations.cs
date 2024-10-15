namespace Talabat.Repository.Data.Config;

internal class DeliveryMethodConfigurations : IEntityTypeConfiguration<DeliveryMethod>
{
    public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
    {
        builder.Property(deliveryMethod => deliveryMethod.Cost)
            .HasColumnType("decimal(18,2)");
    }
}