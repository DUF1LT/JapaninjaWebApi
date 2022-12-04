using Japaninja.Models.Order;

namespace Japaninja.Services.Order;

public interface IOrderService
{
    Task<OrderConfiguration> GetOrderConfigurationAsync(string customerId);

    Task<CreatedOrderInfo> CreateOrderAsync(CreateOrder createOrder);
}