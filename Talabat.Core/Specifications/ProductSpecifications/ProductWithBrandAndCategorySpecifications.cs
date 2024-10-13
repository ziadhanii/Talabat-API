namespace Talabat.Core.Specifications.ProductSpecifications;

public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
{
    public ProductWithBrandAndCategorySpecifications(ProductSpecParams specParams)
        : base(p =>
            (string.IsNullOrEmpty(specParams.Search) || p.Name.ToLower().Contains(specParams.Search)) &&
            (specParams.BrandId.HasValue == false || p.BrandId == specParams.BrandId.Value) &&
            (specParams.CategoryId.HasValue == false || p.CategoryId == specParams.CategoryId))
    {
        Includes.Add(p => p.Brand);
        Includes.Add(c => c.Category);

        if (string.IsNullOrEmpty(specParams.Sort) == false)
        {
            switch (specParams.Sort)
            {
                case "priceAsc":
                    AddOrderBy(p => p.Price);
                    break;
                case "priceDesc":
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    AddOrderBy(p => p.Name);
                    break;
            }
        }
        else
            AddOrderBy(p => p.Name);


        ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
    }


    public ProductWithBrandAndCategorySpecifications(int id)
        : base(p => p.Id == id)
    {
        Includes.Add(p => p.Brand);
        Includes.Add(c => c.Category);
    }
}