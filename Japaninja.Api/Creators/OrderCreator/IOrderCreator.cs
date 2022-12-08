using Japaninja.DomainModel.Models;
using Japaninja.Models.Addresses;
using Japaninja.Models.Order;

namespace Japaninja.Creators.OrderCreator;

public interface IOrderCreator : ICreator
{
    OrderModel CreateFrom(Order order);

    OrderProductModel CreateFrom(OrdersProducts orderProduct);

    OrderCutleryModel CreateFrom(OrdersCutlery orderCutlery);

    CustomerAddressModel CreateFrom(CustomerAddress customerAddress);

    IReadOnlyCollection<OrdersProducts> CreateFrom(Order order, IReadOnlyCollection<OrderProduct> orderProducts);

    IReadOnlyCollection<OrdersCutlery> CreateFrom(Order order, IReadOnlyCollection<OrderCutlery> orderCutlery);
}