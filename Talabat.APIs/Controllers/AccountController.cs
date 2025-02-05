using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Controllers;

public class AccountController(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IAuthService authService,
    IMapper mapper)
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
            Token = await authService.CreateTokenAsync(user, userManager)
        });
    }


    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto model)
    {
        if (CheckEmailExistsAsync(model.Email).Result.Value)
            return BadRequest(new ApiValidationErrorResponse() { Errors = ["Email is already in use."] });

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
            Token = await authService.CreateTokenAsync(user, userManager)
        });
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var user = await userManager.FindByEmailAsync(email);
        return Ok(
            new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await authService.CreateTokenAsync(user, userManager)
            }
        );
    }


    [Authorize]
    [HttpGet("address")]
    public async Task<ActionResult<AddressDto>> GetCurrentAddress()
    {
        var user = await userManager.FindUserWithAddressAsync(User);
        var address = mapper.Map<AddressDto>(user.Address);
        return Ok(address);
    }

    [Authorize]
    [HttpPut("address")]
    public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto updatedAddress)
    {
        var address = mapper.Map<AddressDto, Address>(updatedAddress);
        var user = await userManager.FindUserWithAddressAsync(User);

        if (user == null)
        {
            return NotFound(new ApiResponse(404, "User not found."));
        }

        if (user.Address == null)
        {
            return BadRequest(new ApiResponse(400, "Address not found for the user."));
        }

        address.Id = user.Address.Id;
        user.Address = address;
        
        var result = await userManager.UpdateAsync(user);
        if (result.Succeeded == false)
            return BadRequest(new ApiResponse(400));

        return Ok(updatedAddress);
    }

    [HttpGet("emailexists")]
    public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        => await userManager.FindByEmailAsync(email) is not null;
}