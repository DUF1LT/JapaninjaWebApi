using Japaninja.DomainModel.Models;
using Japaninja.Models.Addresses;
using Japaninja.Models.Order;

namespace Japaninja.Creators.OrderCreator;

public class OrderCreator : IOrderCreator
{
    public OrderModel CreateFrom(Order order)
    {
        return new OrderModel
        {
            Id = order.Id,
            NumberId = order.NumberId,
            Price = order.Price,
            Comment = order.Comment,
            Status = order.Status,
            Cutlery = order.Cutlery.Select(CreateFrom).ToList(),
            Products = order.Products.Select(CreateFrom).ToList(),
            CustomerAddress = order.CustomerAddress is not null ? CreateFrom(order.CustomerAddress) : null,
            Restaurant = order.Restaurant,
            DeliveryTime = order.DeliveryTime,
            DeliveryFactTime = order.DeliveryFactTime,
            CustomerName = order.CustomerName,
            CustomerPhoneNumber = order.CustomerPhoneNumber,
            CourierId = order.CourierId,
            CustomerId = order.CustomerId,
            RestaurantId = order.RestaurantId,
            CustomerAddressId = order.CustomerAddressId
        };
    }

    public OrderProductModel CreateFrom(OrdersProducts orderProduct)
    {
        return new OrderProductModel
        {
            Product = orderProduct.Product,
            Amount = orderProduct.Amount,
        };
    }

    public OrderCutleryModel CreateFrom(OrdersCutlery orderCutlery)
    {
        return new OrderCutleryModel
        {
            Cutlery = orderCutlery.Cutlery,
            Amount = orderCutlery.Amount,
        };
    }

    public CustomerAddressModel CreateFrom(CustomerAddress customerAddress)
    {
        return new CustomerAddressModel
        {
            Id = customerAddress.Id,
            Address = customerAddress.Address,
            CustomerId = customerAddress.CustomerId,
        };
    }

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