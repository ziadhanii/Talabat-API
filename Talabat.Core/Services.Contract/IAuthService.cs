using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;

namespace Talabat.Core.Reposistories.Contract;

public interface IAuthService
{
    Task<string> CreateTokenAsync(AppUser user , UserManager<AppUser> userManager);
}