using Japaninja.Authorization.Requirements;
using Japaninja.Authorization.Requirements.Roles;
using Microsoft.AspNetCore.Authorization;

namespace Japaninja.Authorization;

public static class AuthorizationPolicyBuilderExtensions
{
    public static AuthorizationPolicyBuilder IsManager(this AuthorizationPolicyBuilder authorizationPolicyBuilder)
    {
        return authorizationPolicyBuilder.RequireAuthenticatedUser().AddRequirements(new IsManagerRequirement());
    }

    public static AuthorizationPolicyBuilder IsCustomer(this AuthorizationPolicyBuilder authorizationPolicyBuilder)
    {
        return authorizationPolicyBuilder.RequireAuthenticatedUser().AddRequirements(new IsCustomerRequirement());
    }

    public static AuthorizationPolicyBuilder IsCourier(this AuthorizationPolicyBuilder authorizationPolicyBuilder)
    {
        return authorizationPolicyBuilder.RequireAuthenticatedUser().AddRequirements(new IsCourierRequirement());
    }

    public static AuthorizationPolicyBuilder IsCourierOrManager(this AuthorizationPolicyBuilder authorizationPolicyBuilder)
    {
        return authorizationPolicyBuilder.RequireAuthenticatedUser().AddRequirements(new IsCourierOrManagerRequirement());
    }
}