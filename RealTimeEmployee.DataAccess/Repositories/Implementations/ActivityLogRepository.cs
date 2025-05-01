using Microsoft.EntityFrameworkCore;
using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.Enums;
using RealTimeEmployee.DataAccess.Repositories.Interfaces;

namespace RealTimeEmployee.DataAccess.Repositories.Implementations;

public class ActivityLogRepository : Repository<ActivityLog>, IActivityLogRepository
{
    public ActivityLogRepository(DbContext context) : base(context) { }

    public async Task<IEnumerable<ActivityLog>> GetByEmployeeAndDateRangeAsync(Guid employeeId, DateTime start, DateTime end)
        => await _dbSet
            .Where(log => log.EmployeeId == employeeId && log.StartTime >= start && log.StartTime <= end)
            .ToListAsync();

    public async Task<IEnumerable<ActivityLog>> GetByTypeAsync(ActivityType type)
        => await _dbSet.Where(log => log.ActivityType == type).ToListAsync();
}