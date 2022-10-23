using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Japaninja.Models.Auth;
using Japaninja.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

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


    public JWTTokenDescriptor GenerateToken(string userId)
    {
        var expirationDate = DateTime.UtcNow.AddSeconds(_tokenLifespan);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, userId)
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