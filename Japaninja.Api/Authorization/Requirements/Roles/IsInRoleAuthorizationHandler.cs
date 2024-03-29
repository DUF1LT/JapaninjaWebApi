﻿using System.Security.Claims;
using Japaninja.Extensions;
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
        var role = context.User.GetUserRole();

        if (role is not null && role == _role)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}