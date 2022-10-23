using Japaninja.DomainModel.Identity;
using Japaninja.Models.Auth;
using Microsoft.AspNetCore.Identity;

namespace Japaninja.Services.User;

public interface ICustomerService
{
    Task<CustomerUser> GetCustomerByIdAsync(string id);

    Task<CustomerUser> GetCustomerByEmailAsync(string email);

    Task<string> AddCustomerAsync( string email, string password);
}