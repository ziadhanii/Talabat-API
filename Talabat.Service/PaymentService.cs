using Microsoft.Extensions.Configuration;
using Stripe;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Order_Aggregate;
using Talabat.Core.Specifications.Order_Specifications;
namespace Talabat.Service;

public class PaymentService : IPaymentService
{
    private readonly IConfiguration _configuration;
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentService(IConfiguration configuration, IBasketRepository basketRepository, IUnitOfWork unitOfWork)
    {
        _configuration = configuration;
        _basketRepository = basketRepository;
        _unitOfWork = unitOfWork;
        StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
    }

    public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
    {
        var basket = await _basketRepository.GetBasketAsync(basketId);
        if (basket == null) return null;

        var shippingPrice = 0m;

        if (basket.DeliveryMethodId.HasValue)
        {
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>()
                .GetByIdAsync(basket.DeliveryMethodId.Value);
            if (deliveryMethod != null)
            {
                shippingPrice = deliveryMethod.Cost;
                basket.ShippingPrice = shippingPrice;
            }
        }

        if (basket?.Items.Count > 0)
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.Repository<Core.Entities.Product>().GetByIdAsync(item.Id);
                if (item.Price != product.Price) item.Price = product.Price;
            }

        var service = new PaymentIntentService();
        PaymentIntent paymentIntent;

        if (string.IsNullOrEmpty(basket.PaymentIntentId))
        {
            var createOptions = new PaymentIntentCreateOptions
            {
                Amount = (long)((basket.Items.Sum(item => item.Price * item.Quantity) + shippingPrice) *
                                100),
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" }
            };

            paymentIntent = await service.CreateAsync(createOptions);
            basket.PaymentIntentId = paymentIntent.Id;
            basket.ClientSecret = paymentIntent.ClientSecret;
        }
        else
        {
            var updateOptions = new PaymentIntentUpdateOptions
            {
                Amount = (long)((basket.Items.Sum(item => item.Price * item.Quantity) + shippingPrice) *
                                100),
            };

            paymentIntent = await service.UpdateAsync(basket.PaymentIntentId, updateOptions);
        }

        await _basketRepository.UpdateBasketAsync(basket);
        return basket;
    }

    public async Task<Order> UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSucceeded)
    {
        var spec = new OrderWithPaymentSpecifications(paymentIntentId);
        var order = await _unitOfWork.Repository<Order>().GetEntityByIdWithSpecAsync(spec);
        order.Status = isSucceeded ? OrderStatus.PaymentReceived : OrderStatus.PaymentFailed;
        _unitOfWork.Repository<Order>().Update(order);
        await _unitOfWork.CompleteAsync();
        return order;
    }
}