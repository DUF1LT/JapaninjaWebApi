using System.ComponentModel.DataAnnotations;

namespace Japaninja.Models.Auth;

public class RegisterCustomerUser
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    [Compare("Password")]
    public string RepeatPassword { get; set; }
}