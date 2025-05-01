using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.DataAccess.Enums;

namespace RealTimeEmployee.BusinessLogic.Services.Interfaces;

public interface IActivityLogService
{
    Task LogActivityAsync(Guid employeeId, ActivityType activityType, string? description = null);

    Task<IEnumerable<ActivityLogDto>> GetEmployeeActivitiesAsync(Guid employeeId, ActivityTimeRangeRequest request);

    Task EndCurrentActivityAsync(Guid employeeId);
}
