using FluentValidation;
using RealTimeEmployee.BusinessLogic.Requests;

namespace RealTimeEmployee.BusinessLogic.Validators;

public class OfficeUpdateRequestValidator : AbstractValidator<OfficeUpdateRequest>
{
    public OfficeUpdateRequestValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Office name cannot exceed 100 characters")
            .When(x => x.Name != null);

        RuleFor(x => x.Address)
            .MaximumLength(200).WithMessage("Address cannot exceed 200 characters")
            .When(x => x.Address != null);

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90")
            .When(x => x.Latitude.HasValue);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180")
            .When(x => x.Longitude.HasValue);

        RuleFor(x => x.Radius)
            .GreaterThan(0).WithMessage("Radius must be greater than 0")
            .When(x => x.Radius.HasValue);
    }
}