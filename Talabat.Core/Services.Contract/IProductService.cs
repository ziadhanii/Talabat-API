using Talabat.Core.Specifications.ProductSpecifications;

namespace Talabat.Core.Reposistories.Contract;

public interface IProductService
{
    Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams SpecParams);
    Task<Product?> GetProductAsync(int productId);
    Task<int> GetCountAsync(ProductSpecParams SpecParams);

    Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();
    Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync();
}