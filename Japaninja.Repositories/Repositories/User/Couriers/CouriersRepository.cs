using Japaninja.DomainModel.Identity;
using Microsoft.EntityFrameworkCore;

namespace Japaninja.Repositories.Repositories.User.Couriers;

public class CouriersRepository : UserRepository<CourierUser>, ICouriersRepository
{
    public CouriersRepository(JapaninjaDbContext dbContext) : base(dbContext)
    { }

    public async Task<IReadOnlyCollection<CourierUser>> GetCouriersAsync()
    {
        var couriers = await DbSet.Include(c => c.Orders).ToListAsync();

        return couriers;
    }
}