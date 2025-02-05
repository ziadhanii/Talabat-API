using Talabat.Core.Order_Aggregate;

namespace Talabat.Core.Specifications.Order_Specifications;

public class OrderSpecifications : BaseSpecifications<Order>
{
    public OrderSpecifications(string buyerEmail)
        : base(o => o.BuyerEmail == buyerEmail)
    {
        Includes.Add(o => o.DeliveryMethod);
        Includes.Add(o => o.Items);

        AddOrderByDescending(o => o.OrderDate);
    }


    public OrderSpecifications(int orderId, string buyerEmail)
        : base(o => o.Id == orderId && o.BuyerEmail == buyerEmail)
    {
        Includes.Add(c => c.DeliveryMethod);
        Includes.Add(c => c.Items);
    }
    
}