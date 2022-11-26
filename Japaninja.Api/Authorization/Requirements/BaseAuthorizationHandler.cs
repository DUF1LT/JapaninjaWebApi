using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Japaninja.Authorization.Requirements;

public abstract class BaseAuthorizationHandler<T> : AuthorizationHandler<T>
    where T: IAuthorizationRequirement
{
    protected readonly UserManager<IdentityUser> UserManager;

    protected BaseAuthorizationHandler(UserManager<IdentityUser> userManager)
    {
        UserManager = userManager;
    }
}