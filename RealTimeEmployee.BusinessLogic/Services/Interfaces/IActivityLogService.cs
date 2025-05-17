using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.DataAccess.Enums;
using RealTimeEmployee.DataAccess.Models;

namespace RealTimeEmployee.BusinessLogic.Services.Interfaces;

public interface IActivityLogService
{
    Task LogActivityAsync(Guid employeeId, ActivityType activityType, string? description = null);
    Task<IEnumerable<ActivityLogDto>> GetEmployeeActivitiesAsync(Guid employeeId, ActivityTimeRangeRequest request);
    Task<PaginatedResult<ActivityLogDto>> GetEmployeeActivitiesAsync(Guid employeeId, ActivityTimeRangeRequest request, PaginationRequest pagination);
    Task EndCurrentActivityAsync(Guid employeeId);
}