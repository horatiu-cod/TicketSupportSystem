using FluentValidation;
using TicketSupportSystemApp.ApiService.DTOs.Requests;

namespace TicketSupportSystemApp.ApiService.Validators;

public class LoginValidator : AbstractValidator<UserLoginDTO>
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
