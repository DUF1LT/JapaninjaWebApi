using Japaninja.DomainModel.Identity;

namespace Japaninja.Repositories.Repositories.User.Couriers;

public interface ICouriersRepository
{
    Task<CourierUser> GetCourierAsync(string id);

    Task<IReadOnlyCollection<CourierUser>> GetCouriersAsync();

    Task<(int OrdersAmount, int Rating)> GetCourierOrdersInfoAsync(string id);
}