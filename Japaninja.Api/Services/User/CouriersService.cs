using Japaninja.DomainModel.Identity;
using Japaninja.Logging;
using Japaninja.Repositories.Constants;
using Japaninja.Repositories.Repositories.User.Couriers;
using Japaninja.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace Japaninja.Services.User;

public class CouriersService : ICouriersService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public CouriersService(
        IUnitOfWork unitOfWork,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<CourierUser> GetCourierByIdAsync(string id)
    {
        var customerRepository = _unitOfWork.GetRepository<CourierUser, CouriersRepository>();

        return await customerRepository.GetByIdAsync(id);
    }

    public async Task<CourierUser> GetCourierByEmailAsync(string email)
    {
        var couriersRepository = _unitOfWork.GetRepository<CourierUser, CouriersRepository>();

        return await couriersRepository.GetByEmailAsync(email);
    }

    public async Task<string> AddCourierAsync(string email, string password)
    {
        var courierId = Guid.NewGuid().ToString();
        var courier = new CourierUser
        {
            Id = courierId,
            UserName = email,
            Email = email,
        };

        var result = await _userManager.CreateAsync(courier, password);
        if (!result.Succeeded)
        {
            LoggerContext.Current.LogError($"Failed to create courier {courier.Email} - {result.Errors.Select(e => e.Code)}");

            return null;
        }

        await _userManager.AddToRoleAsync(courier, Roles.Courier);

        return courierId;
    }

    public async Task<bool> DeleteCourierAsync(string id)
    {
        var courier = await GetCourierByIdAsync(id);
        if (courier is null)
        {
            LoggerContext.Current.LogError("Failed to delete courier with id {CourierId}", id);

            return false;
        }

        var result = await _userManager.DeleteAsync(courier);
        if (!result.Succeeded)
        {
            LoggerContext.Current.LogError($"Failed to delete courier {courier.Email} - {result.Errors.Select(e => e.Code)}");

            return false;
        }

        return true;
    }
}