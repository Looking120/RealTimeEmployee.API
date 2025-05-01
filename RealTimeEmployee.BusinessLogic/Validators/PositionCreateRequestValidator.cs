using FluentValidation;
using RealTimeEmployee.BusinessLogic.Requests;

namespace RealTimeEmployee.BusinessLogic.Validators;

public class PositionCreateRequestValidator : AbstractValidator<PositionCreateRequest>
{
    public PositionCreateRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Position title cannot exceed 100 characters");

        RuleFor(x => x.DepartmentId)
            .NotEmpty()
            .WithMessage("Department ID is required");

        RuleFor(x => x.BaseSalary)
            .GreaterThanOrEqualTo(0)
            .When(x => x.BaseSalary.HasValue)
            .WithMessage("Base salary cannot be negative");
    }
}