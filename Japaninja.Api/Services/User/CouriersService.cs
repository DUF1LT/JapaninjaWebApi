using Japaninja.DomainModel.Identity;
using Japaninja.Logging;
using Japaninja.Models.User;
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
        IUnitOfWorkFactory<UnitOfWork> unitOfWorkFactory,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _unitOfWork = unitOfWorkFactory.Create();
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IReadOnlyCollection<CourierUser>> GetCouriers()
    {
        var customerRepository = _unitOfWork.GetRepository<CourierUser, CouriersRepository>();

        var couriers = await customerRepository.GetCouriersAsync();

        return couriers;
    }

    public async Task<CourierUser> GetCourierByIdAsync(string id)
    {
        var couriersRepository = _unitOfWork.GetRepository<CourierUser, CouriersRepository>();

        return await couriersRepository.GetByIdAsync(id);
    }

    public async Task<CourierUser> GetCourierByEmailAsync(string email)
    {
        var couriersRepository = _unitOfWork.GetRepository<CourierUser, CouriersRepository>();

        return await couriersRepository.GetByEmailAsync(email);
    }

    public async Task<(int OrdersAmount, int AverageRating)> GetCourierOrdersInfoAsync(string id)
    {
        var couriersRepository = _unitOfWork.GetRepository<CourierUser, CouriersRepository>();

        return await couriersRepository.GetCourierOrdersInfoAsync(id);
    }

    public async Task<string> AddCourierAsync(RegisterCourierUser registerCourierUser)
    {
        var courierId = Guid.NewGuid().ToString();
        var courier = new CourierUser
        {
            Id = courierId,
            UserName = registerCourierUser.Email,
            Email = registerCourierUser.Email,
            FullName = registerCourierUser.Name,
            PhoneNumber = registerCourierUser.Phone,
        };

        var result = await _userManager.CreateAsync(courier, registerCourierUser.Password);
        if (!result.Succeeded)
        {
            LoggerContext.Current.LogError($"Failed to create courier {courier.Email} - {result.Errors.Select(e => e.Code)}");

            return null;
        }

        await _userManager.AddToRoleAsync(courier, Roles.Courier);
        await _unitOfWork.SaveChangesAsync();

        return courierId;
    }

    public async Task<bool> EditCourierAsync(string id, EditCourierUser editCourierUser)
    {
        var courier = await GetCourierByIdAsync(id);
        if (courier is null)
        {
            LoggerContext.Current.LogError("Failed to edit courier with id {CourierId}", id);

            return false;
        }

        courier.FullName = editCourierUser.Name;
        courier.Image = editCourierUser.Image;
        courier.PhoneNumber = editCourierUser.Phone;

        var couriersRepository = _unitOfWork.GetRepository<CourierUser, CouriersRepository>();
        couriersRepository.Update(courier);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteCourierAsync(string id)
    {
        var couriersRepository = _unitOfWork.GetRepository<CourierUser, CouriersRepository>();

        var courier = await GetCourierByIdAsync(id);
        if (courier is null)
        {
            LoggerContext.Current.LogError("Failed to delete courier with id {CourierId}", id);

            return false;
        }

        couriersRepository.Delete(courier);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}