using Japaninja.DomainModel.Identity;

namespace Japaninja.Services.User;

public interface ICouriersService
{
    Task<CourierUser> GetCourierByIdAsync(string id);

    Task<CourierUser> GetCourierByEmailAsync(string email);

    Task<string> AddCourierAsync(string email, string password);

    Task<bool> DeleteCourierAsync(string id);
}