using Japaninja.DomainModel.Identity;
using Japaninja.Models.User;

namespace Japaninja.Services.User;

public interface ICouriersService
{
    Task<IReadOnlyCollection<CourierUser>> GetCouriers();

    Task<CourierUser> GetCourierByIdAsync(string id);

    Task<CourierUser> GetCourierByEmailAsync(string email);

    Task<(int OrdersAmount, int AverageRating)> GetCourierOrdersInfoAsync(string id);

    Task<string> AddCourierAsync(RegisterCourierUser registerCourierUser);

    Task<bool> EditCourierAsync(string id, EditCourierUser editCourierUser);

    Task<bool> DeleteCourierAsync(string id);
}