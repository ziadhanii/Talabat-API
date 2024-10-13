namespace Talabat.APIs.DTOs;

public class ProductToReturnDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PictureUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public int BrandId { get; set; }
    public string Brand { get; set; } = string.Empty;

    public int CategoryId { get; set; }
    public string Category { get; set; } = string.Empty;
}