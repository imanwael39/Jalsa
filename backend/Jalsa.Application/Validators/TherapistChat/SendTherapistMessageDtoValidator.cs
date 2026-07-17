using FluentValidation;
using Jalsa.Application.DTOs.TherapistChat;

namespace Jalsa.Application.Validators.TherapistChat;

public class SendTherapistMessageDtoValidator : AbstractValidator<SendTherapistMessageDto>
{
    public SendTherapistMessageDtoValidator()
    {
        RuleFor(x => x.ConversationId)
            .NotEmpty().WithMessage("ConversationId is required.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MaximumLength(10000).WithMessage("Content must not exceed 10000 characters.");
    }
}
