using Japaninja.DomainModel.Identity;

namespace Japaninja.Repositories.Repositories.User.Customer;

public class CustomerRepository : UserRepository<CustomerUser>, ICustomerRepository
{
    public CustomerRepository(JapaninjaDbContext dbContext)
        : base(dbContext)
    { }
}