namespace Talabat.APIs.Controllers;

public class ProductController(IGenericReposistory<Product> productRepo) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await productRepo.GetAllAsync();

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await productRepo.GetAsync(id);

        if (product is null)
            return NotFound();

        return Ok(product);
    }
}