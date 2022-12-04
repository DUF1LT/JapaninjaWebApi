using Japaninja.DomainModel.Identity;
using Japaninja.DomainModel.Models;
using Microsoft.EntityFrameworkCore;

namespace Japaninja.Repositories.Repositories.User.Customers;

public class CustomersRepository : UserRepository<CustomerUser>, ICustomersRepository
{
    public CustomersRepository(JapaninjaDbContext dbContext)
        : base(dbContext)
    { }

    public async Task<IReadOnlyCollection<CustomerAddress>> GetCustomerAddressesAsync(string customerId)
    {
        var customer = await DbSet.Include(c => c.Addresses)
            .FirstOrDefaultAsync(c => c.Id == customerId);

        return customer?.Addresses;
    }
}