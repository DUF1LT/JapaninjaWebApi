using Japaninja.DomainModel.Identity;
using Japaninja.Models.User;

namespace Japaninja.Creators.CourierUserCreator;

public class CourierUserCreator : ICourierUserCreator
{
    public CourierUserModel CreateFrom(CourierUser courierUser)
    {
        return new CourierUserModel
        {
            Id = courierUser.Id,
            Email = courierUser.Email,
            FullName = courierUser.FullName,
            PhoneNumber = courierUser.PhoneNumber,
        };
    }
}