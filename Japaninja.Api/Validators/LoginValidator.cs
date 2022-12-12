using FluentValidation;
using Japaninja.Models.Auth;

namespace Japaninja.Validators;

public class LoginValidator : AbstractValidator<LoginUser>
{
    public LoginValidator()
    {
        RuleFor(l => l.Email).NotEmpty();
        RuleFor(l => l.Email).EmailAddress();
        RuleFor(l => l.Password).NotEmpty();
    }
}