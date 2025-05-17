using FluentValidation;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.DataAccess.Constants;

namespace RealTimeEmployee.BusinessLogic.Validators;

public class UpdateRoleRequestValidator : AbstractValidator<UpdateRoleRequest>
{
    public UpdateRoleRequestValidator()
    {
        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("Role name is required")
            .Must(BeValidRole).WithMessage("Role must be either 'Admin' or 'User'");
    }

    private bool BeValidRole(string roleName)
    {
        return roleName == Roles.Admin || roleName == Roles.User;
    }
}