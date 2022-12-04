using Japaninja.DomainModel.Models;
using Japaninja.Models.Order;

namespace Japaninja.Creators.OrderCreator;

public class OrderCreator : IOrderCreator
{
    public IReadOnlyCollection<OrdersProducts> CreateFrom(Order order, IReadOnlyCollection<OrderProduct> orderProducts)
    {
        var ordersProducts = orderProducts.Select(p => new OrdersProducts
        {
            Id = Guid.NewGuid().ToString(),
            ProductId = p.ProductId,
            OrderId = order.Id,
            Amount = p.Amount,
        }).ToList();

        return ordersProducts;
    }

    public IReadOnlyCollection<OrdersCutlery> CreateFrom(Order order, IReadOnlyCollection<OrderCutlery> orderCutlery)
    {
        var ordersCutlery = orderCutlery.Select(p => new OrdersCutlery
        {
            Id = Guid.NewGuid().ToString(),
            CutleryId = p.CutleryId,
            OrderId = order.Id,
            Amount = p.Amount,
        }).ToList();

        return ordersCutlery;
    }
}