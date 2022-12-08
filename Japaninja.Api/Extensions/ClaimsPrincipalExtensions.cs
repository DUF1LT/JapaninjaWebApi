using System.Security.Claims;

namespace Japaninja.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        return GetClaimValue(principal, ClaimTypes.NameIdentifier);
    }

    public static string GetUserRole(this ClaimsPrincipal principal)
    {
        return GetClaimValue(principal, ClaimTypes.Role);
    }

    public static string GetClaimValue(this ClaimsPrincipal principal, string valueType)
    {
        return principal.FindFirst(valueType)?.Value;
    }

}