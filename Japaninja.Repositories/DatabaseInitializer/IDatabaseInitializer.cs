using Microsoft.EntityFrameworkCore;

namespace Japaninja.Repositories.DatabaseInitializer;

public interface IDatabaseInitializer<in TDbContext> where TDbContext : DbContext
{
    void Initialize(TDbContext context);
}