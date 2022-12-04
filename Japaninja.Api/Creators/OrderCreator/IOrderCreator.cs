using Japaninja.DomainModel.Models;
using Japaninja.Models.Order;

namespace Japaninja.Creators.OrderCreator;

public interface IOrderCreator : ICreator
{
    IReadOnlyCollection<OrdersProducts> CreateFrom(Order order, IReadOnlyCollection<OrderProduct> orderProducts);

    IReadOnlyCollection<OrdersCutlery> CreateFrom(Order order, IReadOnlyCollection<OrderCutlery> orderCutlery);
}