using Microsoft.AspNetCore.Identity;

namespace Japaninja.Authorization.Requirements.Roles;

public class IsCourierOrManagerRequirementHandler : IsInRolesAuthorizationHandler<IsCourierOrManagerRequirement>
{
    public IsCourierOrManagerRequirementHandler(UserManager<IdentityUser> userManager)
        : base(userManager, Repositories.Constants.Roles.Courier, Repositories.Constants.Roles.Manager)
    { }
}