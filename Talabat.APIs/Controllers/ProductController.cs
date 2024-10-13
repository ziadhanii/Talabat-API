namespace Talabat.APIs.Controllers;

public class ProductsController(
    IGenericRepository<Product> productRepo,
    IGenericRepository<ProductBrand> brandRepo,
    IGenericRepository<ProductCategory> categoryRepo,
    IMapper mapper
) : BaseApiController
{
    [ProducesResponseType(typeof(IReadOnlyList<ProductToReturnDto>), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<Pagination<Product>>> GetProducts([FromQuery] ProductSpecParams SpecParams)
    {
        var spec = new ProductWithBrandAndCategorySpecifications(SpecParams);
        var products = await productRepo.GetAllWithSpecAsync(spec);

        var data = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

        var countSpec = new ProductWithFilterationForCountSpecifications(SpecParams);
        var count = await productRepo.GetCountAsync(countSpec);
        return Ok(new Pagination<ProductToReturnDto>(SpecParams.PageIndex, SpecParams.PageSize, count, data));
    }


    [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var spec = new ProductWithBrandAndCategorySpecifications(id);
        var product = await productRepo.GetEntityWithSpecAsync(spec);

        if (product is null)
            return NotFound(new ApiResponse(404));

        return Ok(mapper.Map<Product, ProductToReturnDto>(product));
    }


    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
    {
        var brands = await brandRepo.GetAllAsync();
        return Ok(brands);
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
    {
        var categories = await categoryRepo.GetAllAsync();
        return Ok(categories);
    }
}