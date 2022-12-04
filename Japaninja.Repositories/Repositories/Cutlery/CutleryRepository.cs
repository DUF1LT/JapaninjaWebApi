using Microsoft.EntityFrameworkCore;

namespace Japaninja.Repositories.Repositories.Cutlery;

public class CutleryRepository : Repository<DomainModel.Models.Cutlery>, ICutleryRepository
{
    public CutleryRepository(DbContext dbContext) : base(dbContext)
    {
    }
}