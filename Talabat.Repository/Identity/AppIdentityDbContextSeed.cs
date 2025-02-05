using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity;

public static class AppIdentityDbContextSeed
{
    public static async Task SeedUsersAsync(UserManager<AppUser> _userManager)
    {
        if (_userManager.Users.Count() == 0)
        {
            var user = new AppUser
            {
                DisplayName = "Ziad Hany",
                Email = "ziadhani64@gmail.com",
                UserName = "ziad.hany",
                PhoneNumber = "01554530991"
            };

            await _userManager.CreateAsync(user, "Pa$$w0rd");
        }
    }
}