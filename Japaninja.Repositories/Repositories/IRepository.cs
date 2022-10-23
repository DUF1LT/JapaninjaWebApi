using Japaninja.DomainModel.Models.Interfaces;

namespace Japaninja.Repositories.Repositories;

public interface IRepository<in T> where T : class
{
    void Add(T entity);

    void AddRange(IReadOnlyCollection<T> entities);

    void Update(T entity);

    void Delete(T entity);
}