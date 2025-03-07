using FluentValidation;
using RealTimeEmployee.BusinessLogic.Requests;

namespace RealTimeEmployee.BusinessLogic.Validators;

public class UserSignInValidator : AbstractValidator<UserSignInRequest>
{
    public UserSignInValidator()
    {
        RuleFor(u => u.Email)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress();

        RuleFor(u => u.Password)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty().WithMessage("Password is required")
            .Matches(@"^(?=.*[!?\*._^@$])(?=.*\d)(?=.*[A-Z])(?=.*[^\w\s]).{6,}$").WithMessage("Your password must contain at least one (!? *. _ ^ @ $)., one digit, one uppercase");
    }
}
