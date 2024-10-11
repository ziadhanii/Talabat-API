namespace Talabat.APIs.Controllers;

public class ProductController(IGenericReposistory<Product> productRepo, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var spec = new ProductWithBrandAndCategorySpecifications();
        var products = await productRepo.GetAllWithSpecAsync(spec);
        return Ok(mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(products));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var spec = new ProductWithBrandAndCategorySpecifications(id);
        var product = await productRepo.GetEntityWithSpecAsync(spec);

        if (product is null)
            return NotFound();

        return Ok(mapper.Map<Product, ProductToReturnDto>(product));
    }
}