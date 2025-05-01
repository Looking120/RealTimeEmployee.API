using FluentValidation;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.DataAccess.Enums;

namespace RealTimeEmployee.BusinessLogic.Validators;

public class UpdateLocationRequestValidator : AbstractValidator<UpdateLocationRequest>
{
    public UpdateLocationRequestValidator()
    {
        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Invalid latitude value");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Invalid longitude value");

        RuleFor(x => x.LocationType)
            .IsInEnum()
            .WithMessage("Invalid location type");

        // Validate realistic coordinates (optional)
        When(x => Math.Abs(x.Latitude) > 85 || Math.Abs(x.Longitude) > 175, () =>
        {
            RuleFor(x => x.LocationType)
                .Equal(LocationType.Traveling)
                .WithMessage("Polar/extreme coordinates only allowed for traveling status");
        });
    }
}