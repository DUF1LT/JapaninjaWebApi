namespace Japaninja.Repositories.Repositories.Order;

public interface IOrderRepository
{
    IQueryable<DomainModel.Models.Order> GetFullIncludedQuery();
}