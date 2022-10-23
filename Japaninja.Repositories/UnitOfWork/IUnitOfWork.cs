using Japaninja.DomainModel.Models.Interfaces;
using Japaninja.Repositories.Repositories;

namespace Japaninja.Repositories.UnitOfWork;

public interface IUnitOfWork
{
    TRepository GetRepository<TEntity, TRepository>()
        where TEntity : class, IHasId
        where TRepository : Repository<TEntity>;

    Task SaveChangesAsync();
}