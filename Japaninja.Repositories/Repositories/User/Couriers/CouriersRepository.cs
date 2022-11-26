using Japaninja.DomainModel.Identity;

namespace Japaninja.Repositories.Repositories.User.Couriers;

public class CouriersRepository : UserRepository<CourierUser>, ICouriersRepository
{
    public CouriersRepository(JapaninjaDbContext dbContext) : base(dbContext)
    { }
}