using CryptoHelper;
using Japaninja.Extensions;
using Japaninja.JWT;
using Japaninja.Models.Auth;
using Microsoft.AspNetCore.Identity;

namespace Japaninja.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IJwtGenerator _jwtGenerator;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthService(IJwtGenerator jwtGenerator, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _jwtGenerator = jwtGenerator;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<AuthData> GetAuthDataAsync(string id)
    {
        var tokenDescriptor = _jwtGenerator.GenerateToken(id);
        var role = await _userManager.GetUserRoleByUserId(id);

        return new AuthData
        {
            Id = id,
            Token = tokenDescriptor.Token,
            TokenExpirationTime = ((DateTimeOffset)tokenDescriptor.ExpirationDate).ToUnixTimeMilliseconds(),
            Role = role
        };
    }
}