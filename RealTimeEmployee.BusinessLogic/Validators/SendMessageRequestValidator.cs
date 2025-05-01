using FluentValidation;
using RealTimeEmployee.BusinessLogic.Requests;

namespace RealTimeEmployee.BusinessLogic.Validators;

public class SendMessageRequestValidator : AbstractValidator<SendMessageRequest>
{
    public SendMessageRequestValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Message content is required")
            .MaximumLength(2000)
            .WithMessage("Message cannot exceed 2000 characters")
            .MinimumLength(1)
            .WithMessage("Message cannot be empty");

        // Prevent potentially dangerous content
        RuleFor(x => x.Content)
            .Must(x => !x.Contains("<script>", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Invalid message content")
            .Must(x => !x.Contains("http://") && !x.Contains("https://"))
            .WithMessage("Links are not allowed in messages");
    }
}