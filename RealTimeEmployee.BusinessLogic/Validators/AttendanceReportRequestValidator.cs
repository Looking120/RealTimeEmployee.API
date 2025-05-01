using FluentValidation;
using RealTimeEmployee.BusinessLogic.Requests;

namespace RealTimeEmployee.BusinessLogic.Validators;

public class AttendanceReportRequestValidator : AbstractValidator<AttendanceReportRequest>
{
    public AttendanceReportRequestValidator()
    {
        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(x => x.EndDate)
            .WithMessage("Start date must be before end date");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("End date must be after start date");

        RuleFor(x => x.EndDate.Subtract(x.StartDate).TotalDays)
            .LessThanOrEqualTo(365)
            .WithMessage("Report period cannot exceed 1 year");

        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(new DateTime(2020, 1, 1))
            .WithMessage("Reports only available from 2020 onwards");
    }
}