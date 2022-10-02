using Japaninja.DomainModel.Models.Interfaces;

namespace Japaninja.Repositories.Repositories;

public interface IRepository<in T> where T : class, IHasId
{
    void Add(T entity);

    void Update(T entity);

    void Delete(T entity);
}