using Japaninja.DomainModel.Identity;
using Microsoft.EntityFrameworkCore;

namespace Japaninja.Repositories.Repositories.User.Couriers;

public class CouriersRepository : UserRepository<CourierUser>, ICouriersRepository
{
    public CouriersRepository(JapaninjaDbContext dbContext) : base(dbContext)
    { }

    public async Task<CourierUser> GetCourierAsync(string id)
    {
        var courier = await DbSet
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.Id == id);

        return courier;
    }

    public async Task<IReadOnlyCollection<CourierUser>> GetCouriersAsync()
    {
        var couriers = await DbSet.Include(c => c.Orders).ToListAsync();

        return couriers;
    }

    public async Task<(int OrdersAmount, int Rating)> GetCourierOrdersInfoAsync(string id)
    {
        var info = await DbSet
            .Where(c => c.Id == id)
            .Select(c => new { OrdersAmount = c.Orders.Count, AverageRating = c.Orders.Average(o => o.Order.Rating) })
            .FirstOrDefaultAsync();

        return (info.OrdersAmount, Convert.ToInt32(Math.Round(info.AverageRating ?? 0)));
    }
}