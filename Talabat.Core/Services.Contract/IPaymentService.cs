using Talabat.Core.Order_Aggregate;

namespace Talabat.Core.Reposistories.Contract;

public interface IPaymentService
{
    Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId);
    Task<Order> UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSucceeded);
}