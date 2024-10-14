namespace Talabat.APIs.Controllers;

public class AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    : BaseApiController
{
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);

        if (user is null) return Unauthorized(new ApiResponse(401));

        var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (result.Succeeded is false) return Unauthorized(new ApiResponse(401));

        return Ok(new UserDto
        {
            DisplayName = user.DisplayName,
            Email = user.Email,
            Token = "This will be a token"
        });
    }


    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto model)
    {
        var user = new AppUser()
        {
            DisplayName = model.DisplayName,
            Email = model.Email,
            UserName = model.Email.Split("@")[0],
            PhoneNumber = model.PhoneNumber
        };
        var result = await userManager.CreateAsync(user, model.Password);
        if (result.Succeeded is false) return BadRequest(new ApiResponse(400));
        return Ok(new UserDto
        {
            DisplayName = user.DisplayName,
            Email = user.Email,
            Token = "This will be a token"
        });
    }
}