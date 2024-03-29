﻿using Japaninja.Authorization;
using Japaninja.Creators.CourierUserCreator;
using Japaninja.DomainModel.Identity;
using Japaninja.Models.Error;
using Japaninja.Models.User;
using Japaninja.Services.Auth;
using Japaninja.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Japaninja.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CouriersController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ICouriersService _couriersService;
    private readonly ICourierUserCreator _courierUserCreator;

    public CouriersController(
        ICouriersService couriersService,
        IAuthService authService,
        ICourierUserCreator courierUserCreator)
    {
        _couriersService = couriersService;
        _authService = authService;
        _courierUserCreator = courierUserCreator;
    }

    [Authorize(Policy = Policies.IsCourierOrManager)]
    [HttpGet("{id}")]
    public async Task<ActionResult<CourierUserDetailsModel>> GetCourier(string id)
    {
        var courier = await _couriersService.GetCourierByIdAsync(id);
        var courierOrdersInfo = await _couriersService.GetCourierOrdersInfoAsync(id);
        var courierDetailsModel = _courierUserCreator.CreateFrom(courier, courierOrdersInfo.OrdersAmount, courierOrdersInfo.AverageRating);

        return Ok(courierDetailsModel);
    }

    [Authorize(Policy = Policies.IsManager)]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<CourierUserModel>>> GetCouriers()
    {
        var couriers = await _couriersService.GetCouriers();
        var couriersModels = couriers.Select(_courierUserCreator.CreateFrom).ToList();

        return Ok(couriersModels);
    }

    [Authorize(Policy = Policies.IsManager)]
    [HttpPost]
    public async Task<ActionResult<string>> RegisterNewCourier([FromBody] RegisterCourierUser registerCourier)
    {
        var user = await _couriersService.GetCourierByEmailAsync(registerCourier.Email);

        if (user is not null)
        {
            return BadRequest(ErrorResponse.CreateFromApiError(ApiError.UserWithTheSameEmailAlreadyExist));
        }

        var customerId = await _couriersService.AddCourierAsync(registerCourier);
        if (customerId is null)
        {
            return BadRequest();
        }

        return Ok(customerId);
    }

    [Authorize(Policy = Policies.IsCourierOrManager)]
    [HttpPut("{id}")]
    public async Task<ActionResult<string>> EditCourier(string id, [FromBody] EditCourierUser editCourierUser)
    {
        var courierUser = await _couriersService.GetCourierByIdAsync(id);

        if (courierUser is null)
        {
            return NotFound(ErrorResponse.CreateFromApiError(ApiError.UserDoesNotExist));
        }

        var isEditSuccessful = await _couriersService.EditCourierAsync(id, editCourierUser);
        if (!isEditSuccessful)
        {
            return BadRequest();
        }

        return Ok();
    }

    [Authorize(Policy = Policies.IsManager)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> FireCourier(string id)
    {
        var user = await _couriersService.GetCourierByIdAsync(id);
        if (user is null)
        {
            return BadRequest(ErrorResponse.CreateFromApiError(ApiError.UserDoesNotExist));
        }

        var isDeleteSuccessful = await _couriersService.DeleteCourierAsync(id);
        if (!isDeleteSuccessful)
        {
            return BadRequest();
        }

        return Ok();
    }
}