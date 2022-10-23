using Microsoft.AspNetCore.Identity;

namespace Japaninja.Repositories.Repositories.User;

public interface IUserRepository<T> : IRepository<T> where T : IdentityUser
{
    Task<T> GetByEmailAsync(string email);
}