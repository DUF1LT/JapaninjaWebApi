using Japaninja.DomainModel.Models;

namespace Japaninja.Repositories.Repositories.User.Customers;

public interface ICustomersRepository
{
    Task<IReadOnlyCollection<CustomerAddress>> GetCustomerAddressesAsync(string customerId);
}