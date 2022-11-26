using Microsoft.AspNetCore.Identity;

namespace Japaninja.Authorization.Requirements.Roles;

public class IsManagerRequirementHandler : IsInRoleAuthorizationHandler<IsManagerRequirement>
{
    public IsManagerRequirementHandler(UserManager<IdentityUser> userManager)
        : base(userManager, Repositories.Constants.Roles.Manager)
    { }
}