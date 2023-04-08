using Japaninja.Authorization;
using Japaninja.Creators.OrderCreator;
using Japaninja.DomainModel.Models;
using Japaninja.DomainModel.Models.Enums;
using Japaninja.Extensions;
using Japaninja.Models.Error;
using Japaninja.Models.Order;
using Japaninja.Repositories.Constants;
using Japaninja.Services.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Japaninja.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IOrderCreator _orderCreator;

    public OrdersController(IOrderService orderService, IOrderCreator orderCreator)
    {
        _orderService = orderService;
        _orderCreator = orderCreator;
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
    public async Task<ActionResult<OrderModel>> Get(string id)
    {
        var order = await _orderService.GetOrderAsync(id);
        if (order is null)
        {
            return NotFound();
        }

        var orderModel = _orderCreator.CreateFrom(order);

        return Ok(orderModel);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<OrderModel>>> Get(OrderStatus orderStatus)
    {
        var orders = await _orderService.GetOrdersAsync(orderStatus);

        var orderModels = orders.Select(_orderCreator.CreateFrom).ToList();

        return Ok(orderModels);
    }

    [Authorize(Policy = Policies.IsCustomer)]
    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IReadOnlyCollection<OrderModel>>> GetCustomerOrders(string customerId, bool isActiveOrders = true)
    {
        var orders = await _orderService.GetCustomerOrdersAsync(customerId, isActiveOrders);

        var orderModels = orders.Select(_orderCreator.CreateFrom).ToList();

        return Ok(orderModels);
    }

    [Authorize]
    [HttpGet("courier/{courierId}")]
    public async Task<ActionResult<IReadOnlyCollection<OrderModel>>> GetCourierOrders(string courierId, OrderStatus orderStatus)
    {
        var orders = await _orderService.GetCouriersOrdersAsync(courierId, orderStatus);

        var orderModels = orders.Select(_orderCreator.CreateFrom).ToList();

        return Ok(orderModels);
    }

    [Authorize(Policy = Policies.IsManager)]
    [HttpPut("{id}/cancel")]
    public async Task<ActionResult> CancelOrder(string id)
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

    [Authorize(Policy = Policies.IsManager)]
    [HttpPut("{id}/process")]
    public async Task<ActionResult> ProcessOrder(string id)
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

    [Authorize(Policy = Policies.IsManager)]
    [HttpPut("{id}/setToReady")]
    public async Task<ActionResult> SetToReadyOrder(string id)
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

    [Authorize(Policy = Policies.IsCourier)]
    [HttpPut("{id}/ship")]
    public async Task<ActionResult> ShipOrder(string id)
    {
        var courierId = User.GetUserId();

        var order = await _orderService.GetOrderAsync(id);
        if (order is null)
        {
            return NotFound();
        }

        if (order.Status != OrderStatus.Ready)
        {
            return BadRequest(ErrorResponse.CreateFromApiError(ApiError.OrderShouldBeInReadyStatus));
        }

        await _orderService.ShipOrderAsync(id, courierId);

        return Ok();
    }

    [Authorize(Policy = Policies.IsCourierOrManager)]
    [HttpPut("{id}/close")]
    public async Task<ActionResult> CloseOrder(string id)
    {
        var userId = User.GetUserId();
        var userRole = User.GetUserRole();

        var order = await _orderService.GetOrderAsync(id);
        if (order is null)
        {
            return NotFound();
        }

        switch (userRole)
        {
            case Roles.Manager:
            {
                if (order.Status != OrderStatus.Ready)
                {
                    return BadRequest(ErrorResponse.CreateFromApiError(ApiError.OrderShouldBeInReadyStatus));
                }

                if (order.CustomerAddress is not null)
                {
                    return BadRequest(ErrorResponse.CreateFromApiError(ApiError.CanNotCloseOrderInReadyStatusThatIsNotPickup));
                }

                break;
            }
            case Roles.Courier:
            {
                if (order.Status != OrderStatus.Shipping)
                {
                    return BadRequest(ErrorResponse.CreateFromApiError(ApiError.OrderShouldBeInShippingStatus));
                }

                if (order.CourierId != userId)
                {
                    return BadRequest(ErrorResponse.CreateFromApiError(ApiError.CanNotCloseOrderThatNotBelongsToYou));
                }
                break;
            }
        }

        await _orderService.CloseOrderAsync(id);

        return Ok();
    }

    [Authorize(Policy = Policies.IsCustomer)]
    [HttpPut("{id}/rate")]
    public async Task<ActionResult> RateOrder(string id, [FromBody] RateOrder rateOrder)
    {
        var courierId = User.GetUserId();

        var order = await _orderService.GetOrderAsync(id);
        if (order is null)
        {
            return NotFound();
        }

        if (order.Status != OrderStatus.Closed)
        {
            return BadRequest(ErrorResponse.CreateFromApiError(ApiError.OrderShouldBeClosed));
        }

        await _orderService.RateOrderAsync(id, rateOrder.Rating, rateOrder.Feedback);

        return Ok();
    }
}