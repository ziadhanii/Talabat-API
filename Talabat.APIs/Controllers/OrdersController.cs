using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Talabat.Core.Order_Aggregate;
using Order = Talabat.Core.Order_Aggregate.Order;

namespace Talabat.APIs.Controllers;

[Authorize]
public class OrdersController(IOrderService orderService, IMapper mapper) : BaseApiController
{
    [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
    {
        var buyerEmail = User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(buyerEmail)) return BadRequest(new ApiResponse(400, "Email not found"));

        var address = mapper.Map<AddressDto, Address>(orderDto.shipToAddress);
        var order = await orderService.CreateOrderAsync(buyerEmail, orderDto.BasketId,
            orderDto.DeliveryMethodId, address);
        if (order == null) return BadRequest(new ApiResponse(400, "Problem creating order"));

        return Ok(mapper.Map<Order, OrderToReturnDto>(order));
    }

    [ProducesResponseType(typeof(IEnumerable<OrderToReturnDto>), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
    {
        var buyerEmail = User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(buyerEmail)) return BadRequest(new ApiResponse(400, "Email not found"));

        var orders = await orderService.GetOrdersForUserAsync(buyerEmail);
        return Ok(mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
    }

    [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id)
    {
        var buyerEmail = User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(buyerEmail)) return BadRequest(new ApiResponse(400, "Email not found"));

        var order = await orderService.GetOrderByIdForUserAsync(id, buyerEmail);
        if (order == null)
            return NotFound(new ApiResponse(404));

        return Ok(mapper.Map<Order, OrderToReturnDto>(order));
    }

    [HttpGet("deliveryMethod")]
    public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        => Ok(await orderService.GetDeliveryMethodsAsync());
}