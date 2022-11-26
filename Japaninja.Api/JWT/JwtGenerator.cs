using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Japaninja.Common.Options;
using Japaninja.Models.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Japaninja.JWT;

public class JwtGenerator : IJwtGenerator
{
    private readonly SymmetricSecurityKey _secretToken;
    private readonly int _tokenLifespan;

    public JwtGenerator(IOptions<JWTOptions> options)
    {
        _secretToken = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey));
        _tokenLifespan = options.Value.Lifespan;
    }

    public JWTTokenDescriptor GenerateToken(string userId, string role)
    {
        var expirationDate = DateTime.UtcNow.AddSeconds(_tokenLifespan);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Role, role)
        };

        var credentials = new SigningCredentials(_secretToken, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expirationDate,
            SigningCredentials = credentials,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

        return new JWTTokenDescriptor
        {
            Token = token,
            ExpirationDate = expirationDate,
        };
    }
}