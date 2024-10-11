namespace Talabat.APIs.Controllers;

public class BuggyController : BaseApiController
{
    private readonly StoreContext _storeContext;
    public BuggyController(StoreContext storeContext)
    {
        _storeContext = storeContext;
    }

    [HttpGet("bad-request")]
    public ActionResult GetBadRequestError() => BadRequest(new ApiResponse(400));

    [HttpGet("not-found")]
    public ActionResult GetNotFoundError() => NotFound(new ApiResponse(404));

    [HttpGet("unauthorized")]
    public ActionResult GetUnauthorizedError() => Unauthorized(new ApiResponse(401));

    [HttpGet("bad-request/{id}")]
    public ActionResult GetBadRequestError(int id) => Ok();
    
    [HttpGet("server-error")]
    public ActionResult GetServerError()
    {
        var product = _storeContext.Products.Find(100);
        var productToReturn = product.ToString();
        return Ok(productToReturn);
    }

    [HttpGet("validation-error")]
    public ActionResult GetValidationError(Product product)
    {
        ModelState.AddModelError("Name", "The Name field is required.");
        ModelState.AddModelError("Email", "The Email field is invalid.");
        return ValidationProblem(ModelState);
    }
}