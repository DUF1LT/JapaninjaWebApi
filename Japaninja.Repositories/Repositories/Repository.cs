using Microsoft.EntityFrameworkCore;

namespace Japaninja.Repositories.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbContext DbContext;
    protected readonly DbSet<T> DbSet;

    protected Repository(DbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = dbContext.Set<T>();
    }

    public IQueryable<T> GetQuery()
    {
        return DbSet;
    }

    public virtual async Task<T> GetByIdAsync(string id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual void Add(T entity)
    {
        DbSet.Add(entity);
    }

    public virtual void AddRange(IReadOnlyCollection<T> entities)
    {
        DbSet.AddRange(entities);
    }

    public virtual void Update(T entity)
    {
        DbContext.Entry(entity).State = EntityState.Modified;
    }

    public virtual void Delete(T entity)
    {
        DbSet.Remove(entity);
    }
}