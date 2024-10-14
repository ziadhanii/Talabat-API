namespace Talabat.APIs.Controllers;

public class BasketController(IBasketRepository basketRepository, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
    {
        var basket = await basketRepository.GetBasketAsync(id);

        return Ok(basket ?? new CustomerBasket(id));
    }

    [HttpPost]
    public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
    {
        var mappedBasket = mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
        var createdOrUpdatedBasket = await basketRepository.UpdateBasketAsync(mappedBasket);
        if (createdOrUpdatedBasket is null) return BadRequest(new ApiResponse(400));
        return Ok(createdOrUpdatedBasket);
    }

    [HttpDelete]
    public async Task DeleteBasket(string id)
    {
        await basketRepository.DeleteBasketAsync(id);
    }
}