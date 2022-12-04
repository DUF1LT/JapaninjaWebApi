using Microsoft.EntityFrameworkCore;

namespace Japaninja.Repositories.Repositories.Order;

public class OrderRepository : Repository<DomainModel.Models.Order>, IOrderRepository
{
    public OrderRepository(DbContext dbContext) : base(dbContext)
    { }
}