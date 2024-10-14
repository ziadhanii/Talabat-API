namespace Talabat.APIs.DTOs;

public class BasketItemDto
{
    [Required] public int Id { get; set; }

    [Required(ErrorMessage = "Product name is required.")]
    public string ProductName { get; set; }

    [Required(ErrorMessage = "Picture URL is required.")]
    public string PictureUrl { get; set; }

    [Range(0.1, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    [Required(ErrorMessage = "Price is required.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Brand is required.")]
    public string Brand { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    public string Category { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    [Required(ErrorMessage = "Quantity is required.")]
    public int Quantity { get; set; }
}