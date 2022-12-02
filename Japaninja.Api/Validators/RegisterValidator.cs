using FluentValidation;
using Japaninja.Models.Auth;

namespace Japaninja.Validators;

public class RegisterValidator : AbstractValidator<RegisterCustomerUser>
{
    public RegisterValidator()
    {
        RuleFor(l => l.Email).NotEmpty();
        RuleFor(l => l.Email).EmailAddress();
        RuleFor(l => l.Password).NotEmpty();
        RuleFor(l => l.RepeatPassword).NotEmpty().Equal(x => x.Password);
    }
}