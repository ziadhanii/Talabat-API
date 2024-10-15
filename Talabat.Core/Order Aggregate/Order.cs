using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talabat.Core.Order_Aggregate;

public class Order : BaseEntity
{
    public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod,
        ICollection<OrderItem> items, decimal subtotal)
    {
        BuyerEmail = buyerEmail;
        ShippingAddress = shippingAddress;
        DeliveryMethod = deliveryMethod;
        Items = items;
        Subtotal = subtotal;
    }

    public Order()
    { }
    public string BuyerEmail { get; set; }
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public Address ShippingAddress { get; set; }

    public int DeliveryMethodId { get; set; } // Foreign Key [1]
    public DeliveryMethod DeliveryMethod { get; set; } // Navigational Property [One]

    public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
    public decimal Subtotal { get; set; }


    // [NotMapped] public decimal Total => Subtotal + DeliveryMethod.Cost;
    public decimal GetTotal() => Subtotal + DeliveryMethod.Cost;

    public string PaymentIntentId { get; set; } = string.Empty;
}