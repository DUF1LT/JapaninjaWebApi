using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Japaninja.Authorization.Requirements.Roles;

public class IsInRoleAuthorizationHandler<T>: BaseAuthorizationHandler<T> where T : IAuthorizationRequirement
{
    private readonly string _role;

    public IsInRoleAuthorizationHandler(UserManager<IdentityUser> userManager, string role)
        : base(userManager)
    {
        _role = role;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, T requirement)
    {
        var roleClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

        if (roleClaim is not null && roleClaim.Value == _role)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}