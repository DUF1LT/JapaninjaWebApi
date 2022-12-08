using Microsoft.AspNetCore.Identity;

namespace Japaninja.Authorization.Requirements.Roles;

public class IsCustomerRequirementHandler : IsInRoleAuthorizationHandler<IsCustomerRequirement>
{
    public IsCustomerRequirementHandler(UserManager<IdentityUser> userManager)
        : base(userManager, Repositories.Constants.Roles.Customer)
    { }
}