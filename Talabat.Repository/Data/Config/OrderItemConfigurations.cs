using Talabat.Core.Order_Aggregate;

namespace Talabat.Repository.Data.Config;

internal class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(oi => oi.Price)
            .HasColumnType("decimal(18,2)");

        builder.OwnsOne(orderItem => orderItem.Product, Product => Product.WithOwner());
    }
}