using Japaninja.Models.Auth;
using Microsoft.AspNetCore.Identity;

namespace Japaninja.JWT;

public interface IJwtGenerator
{
    JWTTokenDescriptor GenerateToken(string userId, string role);
}