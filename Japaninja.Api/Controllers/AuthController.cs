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
    private readonly ICustomerService _customerService;

    public AuthController(
        SignInManager<IdentityUser> signInManager,
        IAuthService authService,
        ICustomerService customerService,
        UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _authService = authService;
        _customerService = customerService;
        _userManager = userManager;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthData>> Login([FromBody] LoginUser login)
    {
        var user = await _userManager.FindByEmailAsync(login.Email);
        if (user is null)
        {
            return BadRequest(ErrorResponse.CreateFromApiError(ApiError.UserDoesNotExist));
        }

        var isPasswordValidResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash,login.Password);
        if (isPasswordValidResult != PasswordVerificationResult.Success)
        {
            return BadRequest(ErrorResponse.CreateFromApiError(ApiError.PasswordIsInvalid));
        }

        var authData = await _authService.GetAuthDataAsync(user.Id);

        return authData;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthData>> Register([FromBody] RegisterUser register)
    {
        var user = await _customerService.GetCustomerByEmailAsync(register.Email);

        if (user is not null)
        {
            return BadRequest(ErrorResponse.CreateFromApiError(ApiError.UserWithTheSameEmailAlreadyExist));
        }

        var customerId = await _customerService.AddCustomerAsync(register.Email, register.Password);

        var authData = await _authService.GetAuthDataAsync(customerId);

        return authData;
    }
}