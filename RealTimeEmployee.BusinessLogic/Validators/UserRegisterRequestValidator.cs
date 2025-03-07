using FluentValidation;
using RealTimeEmployee.BusinessLogic.Requests;

namespace RealTimeEmployee.BusinessLogic.Validators;

public class UserRegisterRequestValidator : AbstractValidator<UserRegisterRequest>
{
    public UserRegisterRequestValidator()
    {
        RuleFor(u => u.UserName)
        .Cascade(CascadeMode.Stop)
        .NotEmpty();

        RuleFor(u => u.Password)
             .Cascade(CascadeMode.Stop)
             .NotNull()
             .NotEmpty().WithMessage("Password is required")
             .MinimumLength(6).WithMessage("Password must be at least 8 characters")
             .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
             .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
             .Matches("[0-9]").WithMessage("Password must contain at least one digit")
             .Matches("[!@#$%^&*()]").WithMessage("Password must contain at least one special character");

        RuleFor(u => u.ConfirmPassword)
            .Cascade(CascadeMode.Stop)
            .Equal(u => u.Password)
            .WithMessage("Password confirmed must be equal to the password");

        RuleFor(u => u.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress();
    }
}
