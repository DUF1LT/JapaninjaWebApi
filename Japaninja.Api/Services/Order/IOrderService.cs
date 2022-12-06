using Japaninja.DomainModel.Models.Enums;
using Japaninja.Models.Order;

namespace Japaninja.Services.Order;

public interface IOrderService
{
    Task<OrderConfiguration> GetOrderConfigurationAsync(string customerId);

    Task<string> CreateOrderAsync(CreateOrder createOrder);

    Task<DomainModel.Models.Order> GetOrderAsync(string id);

    Task<IReadOnlyCollection<DomainModel.Models.Order>> GetOrdersAsync(OrderStatus orderStatus);

    Task CancelOrderAsync(string orderId);

    Task ProcessOrderAsync(string orderId);

    Task SetToReadyOrderAsync(string orderId);
}