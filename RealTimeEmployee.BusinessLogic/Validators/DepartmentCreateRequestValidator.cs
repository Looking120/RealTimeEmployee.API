using FluentValidation;
using RealTimeEmployee.BusinessLogic.Requests;

namespace RealTimeEmployee.BusinessLogic.Validators;

public class DepartmentCreateRequestValidator : AbstractValidator<DepartmentCreateRequest>
{
    public DepartmentCreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Department name cannot exceed 100 characters");
    }
}