using Japaninja.Models.Order;
using Japaninja.Services.Order;
using Microsoft.AspNetCore.Mvc;

namespace Japaninja.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("configuration/{customerId}")]
    public async Task<OrderConfiguration> GetCustomerOrderConfiguration(string customerId)
    {
       var orderConfiguration = await _orderService.GetOrderConfigurationAsync(customerId);

       return orderConfiguration;
    }

    [HttpPost]
    public async Task<ActionResult<CreatedOrderInfo>> Create([FromBody] CreateOrder createOrder)
    {
        var createdOrderInfo = await _orderService.CreateOrderAsync(createOrder);

        return Ok(createdOrderInfo);
    }
}