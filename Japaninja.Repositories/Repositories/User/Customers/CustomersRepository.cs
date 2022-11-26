using Japaninja.DomainModel.Identity;

namespace Japaninja.Repositories.Repositories.User.Customers;

public class CustomersRepository : UserRepository<CustomerUser>, ICustomersRepository
{
    public CustomersRepository(JapaninjaDbContext dbContext)
        : base(dbContext)
    { }
}