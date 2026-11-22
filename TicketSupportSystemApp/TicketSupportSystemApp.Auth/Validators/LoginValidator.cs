using FluentValidation;
using TicketSupportSystemApp.Auth.DTOs;

namespace TicketSupportSystemApp.Auth.Validators;

public class LoginValidator : AbstractValidator<LoginDTO>
{
    public LoginValidator() 
    {
        RuleFor(ul => ul.Email)
                .NotNull()
                .EmailAddress();
        RuleFor(ut => ut.Password)
                .NotNull()
                .NotEmpty()
                .Length(5, 15);
    }
}
