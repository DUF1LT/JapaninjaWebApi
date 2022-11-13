using Japaninja.DomainModel.Identity;
using Microsoft.AspNetCore.Identity;

namespace Japaninja.Extensions;

public static class UserManagerExtensions
{
    public static async Task<string> GetUserRoleByUserId(this UserManager<IdentityUser> manager, string id)
    {
        var user = await manager.FindByIdAsync(id);
        var roles = await manager.GetRolesAsync(user);

        return roles.FirstOrDefault();
    }
}