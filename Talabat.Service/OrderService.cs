using Talabat.Core.Entities;

namespace Talabat.Service;

public class OrderService : IOrderService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IGenericRepository<Product> _productRepo;
    private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
    private readonly IGenericRepository<Order> _orderRepo;

    public OrderService
    (
        IBasketRepository basketRepository,
        IGenericRepository<Product> productRepo,
        IGenericRepository<DeliveryMethod> deliveryMethodRepo,
        IGenericRepository<Order> orderRepo)
    {
        _basketRepository = basketRepository;
        _productRepo = productRepo;
        _deliveryMethodRepo = deliveryMethodRepo;
        _orderRepo = orderRepo;
    }

    public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId,
        Address shippingAddress)
    {
        var basket = await _basketRepository.GetBasketAsync(basketId);
        var orderItems = new List<OrderItem>();
        if (basket?.Items?.Count > 0)
        {
            foreach (var item in basket.Items)
            {
                var product = await _productRepo.GetAsync(item.Id);
                var productItemOrdered = new ProductItemOrdered(item.Id, product.Name, product.PictureUrl);
                var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);
                orderItems.Add(orderItem);
            }
        }

        var subtotal = orderItems.Sum(orderItem => orderItem.Price * orderItem.Quantity);

        var deliveryMethod = await _deliveryMethodRepo.GetAsync(deliveryMethodId);
        var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subtotal);

        await _orderRepo.AddAsync(order);

        return order;
    }


    public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        throw new NotImplementedException();
    }

    public async Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
    {
        throw new NotImplementedException();
    }
}