using Japaninja.Common.Helpers;
using Japaninja.Common.Options;
using Japaninja.DomainModel.Identity;
using Japaninja.Repositories.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Japaninja.Repositories.DatabaseInitializer;

public class DatabaseInitializer : IDatabaseInitializer<JapaninjaDbContext>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ManagerCredentialsOptions _managerCredentialsOptions;

    public DatabaseInitializer(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<ManagerCredentialsOptions> managerCredentialsOptions)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _managerCredentialsOptions = managerCredentialsOptions.Value;
    }

    public void Initialize(JapaninjaDbContext context)
    {
        context.Database.Migrate();
        Seed(context);
        context.SaveChanges();
    }

    private async void Seed(JapaninjaDbContext context)
    {
        if (_roleManager.FindByNameAsync(Roles.Manager).Result is null)
        {
            await _roleManager.CreateAsync(new IdentityRole(Roles.Manager));
        }

        if (_roleManager.FindByNameAsync(Roles.Customer).Result is null)
        {
           await _roleManager.CreateAsync(new IdentityRole(Roles.Customer));
        }

        if (_roleManager.FindByNameAsync(Roles.Courier).Result is null)
        {
            await _roleManager.CreateAsync(new IdentityRole(Roles.Courier));
        }

        if (_userManager.FindByEmailAsync(_managerCredentialsOptions.Email).Result is null)
        {
            var manager = new ManagerUser
            {
                UserName = _managerCredentialsOptions.Email,
                Email = _managerCredentialsOptions.Email,
            };

            await _userManager.CreateAsync(manager, _managerCredentialsOptions.Password);
            await _userManager.AddToRoleAsync(manager, Roles.Manager);
        }
    }
}