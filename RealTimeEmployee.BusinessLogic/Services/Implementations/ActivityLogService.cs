using FluentValidation;
using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.BusinessLogic.Services.Interfaces;
using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.Enums;
using RealTimeEmployee.DataAccess.Extensions;
using RealTimeEmployee.DataAccess.Repository.Interfaces;

namespace RealTimeEmployee.BusinessLogic.Services.Implementations;

public class ActivityLogService : IActivityLogService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmployeeService _employeeService;
    private readonly IValidator<ActivityTimeRangeRequest> _timeRangeValidator;

    public ActivityLogService(
        IUnitOfWork unitOfWork,
        IEmployeeService employeeService,
        IValidator<ActivityTimeRangeRequest> timeRangeValidator)
    {
        _unitOfWork = unitOfWork;
        _employeeService = employeeService;
        _timeRangeValidator = timeRangeValidator;
    }

    public async Task LogActivityAsync(Guid employeeId, ActivityType activityType, string? description = null)
    {
        await EndCurrentActivityAsync(employeeId);

        // Start new activity
        await _unitOfWork.ActivityLogs.AddAsync(new ActivityLog
        {
            EmployeeId = employeeId,
            ActivityType = activityType,
            StartTime = DateTime.UtcNow,
            Description = description
        });

        await _employeeService.UpdateEmployeeStatusAsync(employeeId, activityType.ToActivityStatus());

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<ActivityLogDto>> GetEmployeeActivitiesAsync(
        Guid employeeId,
        ActivityTimeRangeRequest request)
    {
        await _timeRangeValidator.ValidateAndThrowAsync(request);

        var logs = await _unitOfWork.ActivityLogs.GetByEmployeeAndDateRangeAsync(
            employeeId,
            request.StartDate,
            request.EndDate);

        var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId);

        return logs.Select(log => new ActivityLogDto(
            log.Id,
            employeeId,
            $"{employee.FirstName} {employee.LastName}",
            log.ActivityType,
            log.StartTime,
            log.EndTime,
            log.Description));
    }

    public async Task EndCurrentActivityAsync(Guid employeeId)
    {
        var currentActivity = (await _unitOfWork.ActivityLogs.GetByEmployeeAndDateRangeAsync(
                employeeId,
                DateTime.UtcNow.AddHours(-1),
                DateTime.UtcNow))
            .FirstOrDefault(a => a.EndTime == null);

        if (currentActivity != null)
        {
            currentActivity.EndTime = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync();
        }
    }
}