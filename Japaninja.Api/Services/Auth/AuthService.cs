using CryptoHelper;
using Japaninja.JWT;
using Japaninja.Models.Auth;

namespace Japaninja.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IJwtGenerator _jwtGenerator;

    public AuthService(IJwtGenerator jwtGenerator)
    {
        _jwtGenerator = jwtGenerator;
    }

    public AuthData GetAuthData(string id)
    {
        var tokenDescriptor = _jwtGenerator.GenerateToken(id);

        return new AuthData
        {
            Id = id,
            Token = tokenDescriptor.Token,
            TokenExpirationTime = ((DateTimeOffset)tokenDescriptor.ExpirationDate).ToUnixTimeSeconds(),
        };
    }

    public string HashPassword(string password)
    {
        return Crypto.HashPassword(password);
    }

    public bool VerifyPassword(string actualPassword, string hashedPassword)
    {
        return Crypto.VerifyHashedPassword(hashedPassword, actualPassword);
    }
}