using Talabat.Core.Order_Aggregate;

namespace Talabat.APIs.Helpers;

public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
{
    private readonly IConfiguration _configuration;

    public OrderItemPictureUrlResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
    {
        return !string.IsNullOrEmpty(source.Product.PictureUrl)
            ? $"{_configuration["APIBaseUrl"]}/{source.Product.PictureUrl}"
            : string.Empty;
    }
}