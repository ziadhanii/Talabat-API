using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Order_Aggregate;
using Talabat.Core.Specifications.Order_Specifications;

namespace Talabat.Service;

public class OrderService : IOrderService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentService _paymentService;

    public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IPaymentService paymentService)
    {
        _basketRepository = basketRepository;
        _unitOfWork = unitOfWork;
        _paymentService = paymentService;
    }

    public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId,
        Address shippingAddress)

    {
        var basket = await _basketRepository.GetBasketAsync(basketId);
        var orderItems = new List<OrderItem>();
        if (basket?.Items?.Count > 0)
        {
            var productRepository = _unitOfWork.Repository<Product>();

            foreach (var item in basket.Items)
            {
                var product = await productRepository.GetByIdAsync(item.Id);
                var productItemOrdered = new ProductItemOrdered(item.Id, product.Name, product.PictureUrl);
                var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);
                orderItems.Add(orderItem);
            }
        }

        var subtotal = orderItems.Sum(orderItem => orderItem.Price * orderItem.Quantity);

        var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
        var ordersRepo = _unitOfWork.Repository<Order>();
        var orderSpecs = new OrderWithPaymentSpecifications(basket.PaymentIntentId);
        var existingOrder = await ordersRepo.GetEntityByIdWithSpecAsync(orderSpecs);
        if (existingOrder != null)
        {
            ordersRepo.Delete(existingOrder);

            await _paymentService.CreateOrUpdatePaymentIntent(basketId);
        }

        var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subtotal,
            basket.PaymentIntentId);

        await _unitOfWork.Repository<Order>().AddAsync(order);
        var result = await _unitOfWork.CompleteAsync();
        return result <= 0 ? null : order;
    }


    public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        var ordersRepo = _unitOfWork.Repository<Order>();
        var spec = new OrderWithPaymentSpecifications(buyerEmail);
        var orders = await ordersRepo.GetAllWithSpecAsync(spec);
        return orders;
    }

    public async Task<Order?> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
    {
        var ordersRepo = _unitOfWork.Repository<Order>();
        var orderSpec = new OrderSpecifications(orderId, buyerEmail);
        var order = await ordersRepo.GetEntityByIdWithSpecAsync(orderSpec);
        return order;
    }

    public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
    {
        var deliveryMethodRepo = _unitOfWork.Repository<DeliveryMethod>();
        var deliveryMethods = await deliveryMethodRepo.GetAllAsync();
        return deliveryMethods;
    }
}