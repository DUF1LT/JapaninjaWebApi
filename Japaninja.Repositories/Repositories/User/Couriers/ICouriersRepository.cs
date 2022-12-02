using Japaninja.DomainModel.Identity;

namespace Japaninja.Repositories.Repositories.User.Couriers;

public interface ICouriersRepository
{
    Task<IReadOnlyCollection<CourierUser>> GetCouriersAsync();
}