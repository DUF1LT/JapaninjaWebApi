using Japaninja.DomainModel.Identity;
using Japaninja.Models.User;

namespace Japaninja.Services.User;

public interface ICouriersService
{
    Task<IReadOnlyCollection<CourierUser>> GetCouriers();

    Task<CourierUser> GetCourierByIdAsync(string id);

    Task<CourierUser> GetCourierByEmailAsync(string email);

    Task<string> AddCourierAsync(RegisterCourierUser registerCourierUser);

    Task<bool> DeleteCourierAsync(string id);
}