namespace Talabat.Core.Order_Aggregate;

public class OrderItem : BaseEntity
{
    public OrderItem(ProductItemOrdered product, decimal price, int quantity)
    {
        Product = product;
        Price = price;
        Quantity = quantity;
    }
    public OrderItem()
    {
        
    }
    public ProductItemOrdered Product { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}