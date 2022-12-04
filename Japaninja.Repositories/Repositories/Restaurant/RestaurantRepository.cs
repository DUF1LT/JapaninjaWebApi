using Microsoft.EntityFrameworkCore;

namespace Japaninja.Repositories.Repositories.Restaurant;

public class RestaurantRepository : Repository<DomainModel.Models.Restaurant>, IRestaurantRepository
{
    public RestaurantRepository(DbContext dbContext) : base(dbContext)
    {
    }
}