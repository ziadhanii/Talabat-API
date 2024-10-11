namespace Talabat.Core.Specifications.ProductSpecifications;

public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
{
    public ProductWithBrandAndCategorySpecifications()
        : base()
    {
        Includes.Add(p => p.Brand);
        Includes.Add(c => c.Category);
    }


    public ProductWithBrandAndCategorySpecifications(int id)
        : base(p => p.Id == id)
    {
        Includes.Add(p => p.Brand);
        Includes.Add(c => c.Category);
    }
}