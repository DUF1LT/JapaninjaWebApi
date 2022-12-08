using Japaninja.Authorization;
using Japaninja.Authorization.Requirements;
using Japaninja.Authorization.Requirements.Roles;
using Japaninja.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
                o.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZабвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ0123456789-._@+";
            })
            .AddSignInManager<SignInManager<IdentityUser>>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddUserManager<UserManager<IdentityUser>>()
            .AddEntityFrameworkStores<JapaninjaDbContext>()
            .AddDefaultTokenProviders();
    }

    public static IServiceCollection AddJapaninjaAuthentication(this IServiceCollection services, SymmetricSecurityKey jwtKey)
    {
        services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
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

        return services;
    }

    public static IServiceCollection AddJapaninjaAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(o =>
        {
            o.AddPolicy(Policies.IsManager, p => p.IsManager());
            o.AddPolicy(Policies.IsCustomer, p => p.IsCustomer());
            o.AddPolicy(Policies.IsCourier, p => p.IsCourier());
            o.AddPolicy(Policies.IsCourierOrManager, p => p.IsCourierOrManager());
        });

        services.AddScoped<IAuthorizationHandler, IsManagerRequirementHandler>();
        services.AddScoped<IAuthorizationHandler, IsCustomerRequirementHandler>();
        services.AddScoped<IAuthorizationHandler, IsCourierRequirementHandler>();
        services.AddScoped<IAuthorizationHandler, IsCourierOrManagerRequirementHandler>();

        return services;
    }
}