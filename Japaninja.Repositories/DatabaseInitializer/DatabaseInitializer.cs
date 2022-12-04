using Japaninja.Common.Options;
using Japaninja.DomainModel.Identity;
using Japaninja.DomainModel.Models;
using Japaninja.Repositories.Constants;
using Japaninja.Repositories.Repositories.Cutlery;
using Japaninja.Repositories.Repositories.Restaurant;
using Japaninja.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
namespace Japaninja.Repositories.DatabaseInitializer;

public class DatabaseInitializer : IDatabaseInitializer<JapaninjaDbContext>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ManagerCredentialsOptions _managerCredentialsOptions;
    private readonly RestaurantConfigurationOptions _restaurantConfigurationOptions;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public DatabaseInitializer(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<ManagerCredentialsOptions> managerCredentialsOptions,
        IOptions<RestaurantConfigurationOptions> restaurantConfigurationOptions,
        IUnitOfWorkFactory<UnitOfWork.UnitOfWork> unitOfWorkFactory,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _restaurantConfigurationOptions = restaurantConfigurationOptions.Value;
        _managerCredentialsOptions = managerCredentialsOptions.Value;
        _unitOfWork = unitOfWorkFactory.Create();
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

        var optionsMainRestaurantAddress = _restaurantConfigurationOptions.MainRestaurantAddress;

        var restaurantRepository = _unitOfWork.GetRepository<Restaurant, RestaurantRepository>();
        var mainRestaurant = restaurantRepository
            .GetQuery()
            .FirstOrDefault(r => r.Address == optionsMainRestaurantAddress);

        if (mainRestaurant is null)
        {
            restaurantRepository.Add(new Restaurant
            {
                Id = Guid.NewGuid().ToString(),
                Address = optionsMainRestaurantAddress,
            });

            await _unitOfWork.SaveChangesAsync();
        }

        var cutleryRepository = _unitOfWork.GetRepository<Cutlery, CutleryRepository>();
        var cutlery = cutleryRepository.GetQuery().ToList();

        if (cutlery.Count == 0)
        {
            var availableCutlery = _configuration.GetSection("AvailableCutlery")
                .Get<string[]>()
                .Select((c, index) => new Cutlery
                {
                    Id = index.ToString(),
                    Name = c,
                }).ToList();

            cutleryRepository.AddRange(availableCutlery);

            await _unitOfWork.SaveChangesAsync();
        }

    }
}