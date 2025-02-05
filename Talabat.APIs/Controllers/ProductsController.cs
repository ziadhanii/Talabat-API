namespace Talabat.APIs.Controllers;

public class ProductsController(
    IProductService productService,
    IMapper mapper
) : BaseApiController
{
    [ProducesResponseType(typeof(IReadOnlyList<ProductToReturnDto>), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<Pagination<Product>>> GetProducts([FromQuery] ProductSpecParams SpecParams)
    {
        var products = await productService.GetProductsAsync(SpecParams);
        var count = await productService.GetCountAsync(SpecParams);
        var data = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
        return Ok(new Pagination<ProductToReturnDto>(SpecParams.PageIndex, SpecParams.PageSize, count, data));
    }


    [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await productService.GetProductAsync(id);

        if (product is null)
            return NotFound(new ApiResponse(404));

        return Ok(mapper.Map<Product, ProductToReturnDto>(product));
    }


    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
    {
        var brands = await productService.GetBrandsAsync();
        return Ok(brands);
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
    {
        var categories = await productService.GetCategoriesAsync();
        return Ok(categories);
    }
}