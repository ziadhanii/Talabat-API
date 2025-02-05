using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Specifications.ProductSpecifications;

namespace Talabat.Service;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams SpecParams)
    {
        var spec = new ProductWithBrandAndCategorySpecifications(SpecParams);
        var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
        return products;
    }

    public async Task<Product?> GetProductAsync(int id)
    {
        var spec = new ProductWithBrandAndCategorySpecifications(id);
        var product = await _unitOfWork.Repository<Product>().GetEntityByIdWithSpecAsync(spec);
        return product;
    }

    public async Task<int> GetCountAsync(ProductSpecParams SpecParams)
    {
        var countSpec = new ProductWithFilterationForCountSpecifications(SpecParams);
        var count = await _unitOfWork.Repository<Product>().GetCountAsync(countSpec);
        return count;
    }

    public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
        => await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
  
    public async Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync()
        => await _unitOfWork.Repository<ProductCategory>().GetAllAsync();
}