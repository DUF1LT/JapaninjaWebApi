using Microsoft.EntityFrameworkCore;

namespace Japaninja.Repositories.UnitOfWork;

public class UnitOfWorkFactory : IUnitOfWorkFactory<UnitOfWork>
{
    private readonly DbContextOptions<JapaninjaDbContext> _options;


    public UnitOfWorkFactory(DbContextOptions<JapaninjaDbContext> options)
    {
        _options = options;
    }

    public UnitOfWork Create()
    {
        var dbContext = new JapaninjaDbContext(_options);
        var unitOfWork = new UnitOfWork(dbContext);

        return unitOfWork;
    }
}