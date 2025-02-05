using Talabat.Core.Order_Aggregate;

namespace Talabat.Core.Specifications.Order_Specifications;

public class OrderWithPaymentSpecifications : BaseSpecifications<Order>
{
    public OrderWithPaymentSpecifications(string paymentIntentId)
        : base(o => o.PaymentIntentId == paymentIntentId)
    {
    }
}