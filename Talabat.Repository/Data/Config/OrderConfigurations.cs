using Talabat.Core.Order_Aggregate;
using Order = Talabat.Core.Order_Aggregate.Order;

namespace Talabat.Repository.Data.Config;

internal class OrderConfigurations : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsOne(o => o.ShippingAddress, shippingAddress => { shippingAddress.WithOwner(); });

        builder.HasMany(o => o.Items)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(o => o.Status)
            .HasConversion(
                o => o.ToString(),
                o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o));

        builder.Property(o => o.Subtotal)
            .HasColumnType("decimal(18,2)");

        builder.HasOne(o => o.DeliveryMethod)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);
    }
}