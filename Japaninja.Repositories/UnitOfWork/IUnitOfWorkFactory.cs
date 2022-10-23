namespace Japaninja.Repositories.UnitOfWork;

public interface IUnitOfWorkFactory<T> where T : IUnitOfWork
{
    T Create();
}
