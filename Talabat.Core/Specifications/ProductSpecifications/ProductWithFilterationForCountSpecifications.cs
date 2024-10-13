namespace Talabat.Core.Specifications.ProductSpecifications;

public class ProductWithFilterationForCountSpecifications : BaseSpecifications<Product>
{
    public ProductWithFilterationForCountSpecifications(ProductSpecParams specParams)
        : base(b =>
            (string.IsNullOrEmpty(specParams.Search) || b.Name.ToLower().Contains(specParams.Search)) &&
            (specParams.BrandId.HasValue == false || b.BrandId == specParams.BrandId.Value) &&
            (specParams.CategoryId.HasValue == false || b.CategoryId == specParams.CategoryId))
    {
    }
}