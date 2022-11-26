using Japaninja.Authorization;
using Japaninja.Models.Auth;
using Japaninja.Models.Error;
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

    public CouriersController(
        ICouriersService couriersService,
        IAuthService authService)
    {
        _couriersService = couriersService;
        _authService = authService;
    }

    [Authorize(Policy = Policies.IsManager)]
    [HttpPut]
    public async Task<ActionResult<string>> RegisterNewCourier(RegisterUser courier)
    {
        var user = await _couriersService.GetCourierByEmailAsync(courier.Email);

        if (user is not null)
        {
            return BadRequest(ErrorResponse.CreateFromApiError(ApiError.UserWithTheSameEmailAlreadyExist));
        }

        var customerId = await _couriersService.AddCourierAsync(courier.Email, courier.Password);

        return Ok(customerId);
    }

    [Authorize(Policy = Policies.IsManager)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> FireCourier(string id)
    {
        var user = await _couriersService.GetCourierByIdAsync(id);
        if (user is not null)
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