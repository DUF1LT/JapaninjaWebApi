using Japaninja.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Japaninja.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddJapaninjaIdentity(this IServiceCollection services)
    {
        services.AddIdentity<IdentityUser, IdentityRole>(o =>
            {
                o.Password.RequireNonAlphanumeric = false;
            })
            .AddSignInManager<SignInManager<IdentityUser>>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddUserManager<UserManager<IdentityUser>>()
            .AddEntityFrameworkStores<JapaninjaDbContext>()
            .AddDefaultTokenProviders();
    }

    public static void AddJapaninjaAuthentication(this IServiceCollection services, SymmetricSecurityKey jwtKey)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = jwtKey,
                };
            });
    }
}