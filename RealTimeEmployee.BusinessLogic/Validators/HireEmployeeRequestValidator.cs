using FluentValidation;
using RealTimeEmployee.BusinessLogic.Requests;

namespace RealTimeEmployee.BusinessLogic.Validators;

public class HireEmployeeRequestValidator : AbstractValidator<HireEmployeeRequest>
{
    public HireEmployeeRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User id is required");

        RuleFor(x => x.DepartmentId)
            .NotEmpty().WithMessage("Department id is required");

        RuleFor(x => x.PositionId)
            .NotEmpty().WithMessage("Position id is required");

        RuleFor(x => x.OfficeId)
            .NotEmpty().WithMessage("Office id is required");

        RuleFor(x => x.EmployeeNumber)
            .NotEmpty().WithMessage("Employee number is required")
            .MaximumLength(20).WithMessage("Employee number cannot exceed 20 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^\+?[0-9\s\-$$$$]+$").WithMessage("Invalid phone number format");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(200).WithMessage("Address cannot exceed 200 characters");
    }
}