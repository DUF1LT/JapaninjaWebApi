using Japaninja.Authorization;
using Japaninja.DomainModel.Models;
using Japaninja.DomainModel.Models.Enums;
using Japaninja.Models.Error;
using Japaninja.Models.Order;
using Japaninja.Services.Order;
using Microsoft.AspNetCore.Authorization;
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
    public async Task<ActionResult<string>> Create([FromBody] CreateOrder createOrder)
    {
        var createdOrderId = await _orderService.CreateOrderAsync(createOrder);

        return Ok(createdOrderId);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> Get(string id)
    {
        var order = await _orderService.GetOrderAsync(id);
        if (order is null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    //[Authorize(Policy = Policies.IsManager)]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<Order>>> Get(OrderStatus orderStatus)
    {
        var orders = await _orderService.GetOrdersAsync(orderStatus);

        return Ok(orders);
    }

    //[Authorize(Policy = Policies.IsManager)]
    [HttpPut("{id}/cancel")]
    public async Task<ActionResult<IReadOnlyCollection<Order>>> CancelOrder(string id)
    {
        var order = await _orderService.GetOrderAsync(id);
        if (order is null)
        {
            return NotFound();
        }

        if (order.Status > OrderStatus.Processing)
        {
            return BadRequest(ErrorResponse.CreateFromApiError(ApiError.YouCanNotCancelOrderNotInProcessingStatus));
        }

        await _orderService.CancelOrderAsync(id);

        return Ok();
    }

    //[Authorize(Policy = Policies.IsManager)]
    [HttpPut("{id}/process")]
    public async Task<ActionResult<IReadOnlyCollection<Order>>> ProcessOrder(string id)
    {
        var order = await _orderService.GetOrderAsync(id);
        if (order is null)
        {
            return NotFound();
        }

        if (order.Status != OrderStatus.Processing)
        {
            return BadRequest(ErrorResponse.CreateFromApiError(ApiError.OrderShouldBeInProcessingStatus));
        }

        await _orderService.ProcessOrderAsync(id);

        return Ok();
    }

    //[Authorize(Policy = Policies.IsManager)]
    [HttpPut("{id}/setToReady")]
    public async Task<ActionResult<IReadOnlyCollection<Order>>> SetToReadyOrder(string id)
    {
        var order = await _orderService.GetOrderAsync(id);
        if (order is null)
        {
            return NotFound();
        }

        if (order.Status != OrderStatus.Preparing)
        {
            return BadRequest(ErrorResponse.CreateFromApiError(ApiError.OrderShouldBeInPreparingStatus));
        }

        await _orderService.SetToReadyOrderAsync(id);

        return Ok();
    }
}