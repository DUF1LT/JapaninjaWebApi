using System.ComponentModel.DataAnnotations;

namespace Japaninja.Models.User;

public class RegisterCourierUser
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Phone { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    [Compare("Password")]
    public string RepeatPassword { get; set; }
}