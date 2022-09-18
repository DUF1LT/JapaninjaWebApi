using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Japaninja.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddJapaninjaAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddCookie(IdentityConstants.ApplicationScheme, options =>
            {
                options.Events.OnRedirectToLogin += context =>
                {
                    context.Response.StatusCode = 401;

                    return Task.CompletedTask;
                };
            }).AddCookie(IdentityConstants.ExternalScheme)
            .AddCookie(IdentityConstants.TwoFactorUserIdScheme);;
    }
}