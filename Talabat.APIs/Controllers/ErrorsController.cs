namespace Talabat.APIs.Controllers;

[Route("errors/{code:int}")]
[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorsController : ControllerBase
{
    public ActionResult Error(int code)
    {
        return NotFound(new ApiResponse(code));
    }
}