using FluentValidation;
using RealTimeEmployee.BusinessLogic.Requests;

namespace RealTimeEmployee.BusinessLogic.Validators;

public class ActivityTimeRangeRequestValidator : AbstractValidator<ActivityTimeRangeRequest>
{
    public ActivityTimeRangeRequestValidator()
    {
        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(x => x.EndDate)
            .WithMessage("Start date must be before or equal to end date");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("End date must be after or equal to start date")
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
            .WithMessage("Cannot query future dates");

        RuleFor(x => x.EndDate.Subtract(x.StartDate).TotalDays)
            .LessThanOrEqualTo(31)
            .WithMessage("Time range cannot exceed 31 days");
    }
}