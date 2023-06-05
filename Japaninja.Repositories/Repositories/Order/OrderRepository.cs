using Microsoft.EntityFrameworkCore;

namespace Japaninja.Repositories.Repositories.Order;

public class OrderRepository : Repository<DomainModel.Models.Order>, IOrderRepository
{
    public OrderRepository(DbContext dbContext) : base(dbContext)
    { }

    public IQueryable<DomainModel.Models.Order> GetFullIncludedQuery()
    {
        return DbSet.AsSplitQuery()
            .Include(o => o.CustomerAddress)
            .Include(o => o.Restaurant)
            .Include(o => o.Products)
            .ThenInclude(p => p.Product)
            .Include(o => o.Cutlery)
            .ThenInclude(o => o.Cutlery)
            .Include(o => o.Courier);
    }
}