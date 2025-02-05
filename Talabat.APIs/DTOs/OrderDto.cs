namespace Talabat.APIs.DTOs;

public class OrderDto
{
    [Required(ErrorMessage = "Basket ID is required.")]
    public string BasketId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Delivery method ID is required.")]
    public int DeliveryMethodId { get; set; }

    [Required(ErrorMessage = "Shipping address is required.")]
    public AddressDto shipToAddress { get; set; } = new AddressDto();
}