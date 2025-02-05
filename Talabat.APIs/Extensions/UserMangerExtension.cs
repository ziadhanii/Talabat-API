using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Extensions;

public static class UserMangerExtension
{
    public static async Task<AppUser> FindUserWithAddressAsync(this UserManager<AppUser> userManager,
        ClaimsPrincipal User)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var user = await userManager.Users.Include(u => u.Address).SingleOrDefaultAsync(u => u.Email == email);
        return user;
    }
}