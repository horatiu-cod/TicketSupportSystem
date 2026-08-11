using FluentValidation;
using TicketSupportSystemApp.ApiService.DTOs.Requests;

namespace TicketSupportSystemApp.ApiService.Validators;

public class UpdateCommentValidator : AbstractValidator<UpdateCommentDTO>
{
    public UpdateCommentValidator() 
    {
        RuleFor(uc => uc.Text)
                .NotNull()
                .NotEmpty();
    }
}
