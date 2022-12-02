using System.ComponentModel.DataAnnotations;
using Japaninja.Validators;

namespace Japaninja.Models.Auth;

public class LoginCustomerUser
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}