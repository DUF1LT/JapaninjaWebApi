using System.Security.Claims;
using Japaninja.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Japaninja.Authorization.Requirements.Roles;

public class IsInRolesAuthorizationHandler<T>: BaseAuthorizationHandler<T> where T : IAuthorizationRequirement
{
    private readonly string[] _roles;

    public IsInRolesAuthorizationHandler(UserManager<IdentityUser> userManager, params string[] roles)
        : base(userManager)
    {
        _roles = roles;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, T requirement)
    {
        var role = context.User.GetUserRole();

        if (role is not null && _roles.Contains(role))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}