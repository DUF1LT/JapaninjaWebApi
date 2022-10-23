using FluentValidation;
using Japaninja.Models.Auth;

namespace Japaninja.Validators;

public class RegisterValidator : AbstractValidator<RegisterUser>
{
    public RegisterValidator()
    {
        RuleFor(l => l.Email).NotEmpty();
        RuleFor(l => l.Email).EmailAddress();
        RuleFor(l => l.Password).NotEmpty();
        RuleFor(l => l.ConfirmPassword).NotEmpty().Equal(x => x.Password);
    }
}