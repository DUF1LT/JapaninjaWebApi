using Japaninja.DomainModel.Identity;
using Japaninja.Models.User;

namespace Japaninja.Creators.CourierUserCreator;

public interface ICourierUserCreator : ICreator
{
    CourierUserModel CreateFrom(CourierUser courierUser);
    CourierUserDetailsModel CreateFrom(CourierUser courierUser, int ordersAmount, int averageRating);
}