namespace Talabat.Core.Specifications.ProductSpecifications;

public class ProductSpecParams
{
    private const int MaxPageSize = 10;
    private int pageSize = 5;
    public int PageIndex { get; set; } = 1;

    public int PageSize
    {
        get => pageSize;
        set => pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    private string? search;

    public string? Search
    {
        get => search;
        set => search = value?.ToLower();
    }

    public string? Sort { get; set; }
    public int? BrandId { get; set; }
    public int? CategoryId { get; set; }
}