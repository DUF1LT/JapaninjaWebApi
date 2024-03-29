﻿using Microsoft.AspNetCore.Identity;

namespace Japaninja.Authorization.Requirements.Roles;

public class IsCourierRequirementHandler : IsInRoleAuthorizationHandler<IsCourierRequirement>
{
    public IsCourierRequirementHandler(UserManager<IdentityUser> userManager)
        : base(userManager, Repositories.Constants.Roles.Courier)
    { }
}