using AutoMapper;
using FluentValidation;
using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.BusinessLogic.Services.Interfaces;
using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.Enums;
using RealTimeEmployee.DataAccess.Extensions;
using RealTimeEmployee.DataAccess.Models;
using RealTimeEmployee.DataAccess.Repository.Interfaces;

namespace RealTimeEmployee.BusinessLogic.Services.Implementations;

public class ActivityLogService : IActivityLogService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmployeeService _employeeService;
    private readonly IValidator<ActivityTimeRangeRequest> _timeRangeValidator;
    private readonly IMapper _mapper;

    public ActivityLogService(
        IUnitOfWork unitOfWork,
        IEmployeeService employeeService,
        IValidator<ActivityTimeRangeRequest> timeRangeValidator,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _employeeService = employeeService;
        _timeRangeValidator = timeRangeValidator;
        _mapper = mapper;
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

        return _mapper.Map<IEnumerable<ActivityLogDto>>(logs);
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

    public async Task<PaginatedResult<ActivityLogDto>> GetEmployeeActivitiesAsync(
        Guid employeeId,
        ActivityTimeRangeRequest request,
        PaginationRequest pagination)
    {
        await _timeRangeValidator.ValidateAndThrowAsync(request);

        var logs = await _unitOfWork.ActivityLogs.GetByEmployeeAndDateRangeAsync(
            employeeId,
            request.StartDate,
            request.EndDate);

        var totalCount = logs.Count();

        var pagedLogs = logs
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();

        var logDtos = _mapper.Map<IEnumerable<ActivityLogDto>>(pagedLogs);

        return new PaginatedResult<ActivityLogDto>(
            logDtos,
            totalCount,
            pagination.PageNumber,
            pagination.PageSize);
    }
}