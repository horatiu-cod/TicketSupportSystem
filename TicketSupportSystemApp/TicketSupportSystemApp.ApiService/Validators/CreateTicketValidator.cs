using FluentValidation;
using TicketSupportSystemApp.ApiService.DTOs.Requests;

namespace TicketSupportSystemApp.ApiService.Validators;

public class CreateTicketValidator : AbstractValidator<CreateTicketDTO>
{
    public CreateTicketValidator() 
    {
        RuleFor(ct => ct.Title)
                .NotNull()
                .NotEmpty()
                .Length(0, 100);
        RuleFor(ct => ct.Description)
                .NotNull()
                .NotEmpty();
        RuleFor(ct => ct.Priority)
                .IsInEnum();
        RuleFor(ct => ct.CustomerId)
                .NotNull();
    }
}
