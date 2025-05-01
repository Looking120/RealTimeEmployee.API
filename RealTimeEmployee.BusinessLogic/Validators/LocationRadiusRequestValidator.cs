using FluentValidation;
using RealTimeEmployee.BusinessLogic.Requests;

namespace RealTimeEmployee.BusinessLogic.Validators;

public class LocationRadiusRequestValidator : AbstractValidator<LocationRadiusRequest>
{
    public LocationRadiusRequestValidator()
    {
        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude must be between -90 and 90 degrees");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude must be between -180 and 180 degrees");

        RuleFor(x => x.RadiusKm)
            .GreaterThan(0)
            .WithMessage("Radius must be greater than 0")
            .LessThanOrEqualTo(100)
            .WithMessage("Radius cannot exceed 100 kilometers");
    }
}