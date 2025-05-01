using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.Enums;

namespace RealTimeEmployee.DataAccess.Repositories.Interfaces;

public interface IActivityLogRepository : IRepository<ActivityLog>
{
    /// <summary>
    /// Get logs for an employee between dates
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    Task<IEnumerable<ActivityLog>> GetByEmployeeAndDateRangeAsync(Guid employeeId, DateTime start, DateTime end);

    /// <summary>
    /// Get logs by type (CheckIn, CheckOut, etc.)
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    Task<IEnumerable<ActivityLog>> GetByTypeAsync(ActivityType type);
}