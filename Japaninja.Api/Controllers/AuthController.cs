using Japaninja.Common.Helpers;
using Japaninja.Models.Auth;
using Japaninja.Models.Error;
using Japaninja.Services.Auth;
using Japaninja.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Japaninja.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IAuthService _authService;
    private readonly ICustomersService _customersService;

    public AuthController(
        SignInManager<IdentityUser> signInManager,
        IAuthService authService,
        ICustomersService customersService,
        UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _authService = authService;
        _customersService = customersService;
        _userManager = userManager;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthData>> Login([FromBody] LoginCustomerUser loginCustomer)
    {
        var user = await _userManager.FindByEmailAsync(loginCustomer.Email);
        if (user is null)
        {
            return BadRequest(ErrorResponse.CreateFromApiError(ApiError.UserDoesNotExist));
        }

        var isPasswordValidResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash,loginCustomer.Password);
        if (isPasswordValidResult != PasswordVerificationResult.Success)
        {
            return BadRequest(ErrorResponse.CreateFromApiError(ApiError.PasswordIsInvalid));
        }

        var authData = await _authService.GetAuthDataAsync(user.Id);

        return authData;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthData>> Register([FromBody] RegisterCustomerUser registerCustomer)
    {
        var user = await _customersService.GetCustomerByEmailAsync(registerCustomer.Email);

        if (user is not null)
        {
            return BadRequest(ErrorResponse.CreateFromApiError(ApiError.UserWithTheSameEmailAlreadyExist));
        }

        var customerId = await _customersService.AddCustomerAsync(registerCustomer.Email, registerCustomer.Password);

        var authData = await _authService.GetAuthDataAsync(customerId);

        return authData;
    }
}