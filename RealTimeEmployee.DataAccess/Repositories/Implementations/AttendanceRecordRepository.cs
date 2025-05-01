using Microsoft.EntityFrameworkCore;
using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.Repositories.Interfaces;

namespace RealTimeEmployee.DataAccess.Repositories.Implementations;

public class AttendanceRecordRepository : Repository<AttendanceRecord>, IAttendanceRecordRepository
{
    public AttendanceRecordRepository(DbContext context) : base(context) { }

    public async Task<IEnumerable<AttendanceRecord>> GetByEmployeeAndMonthAsync(
        Guid employeeId,
        int year,
        int month)
        => await _dbSet
            .Where(a =>
                a.EmployeeId == employeeId &&
                a.CheckInTime.Year == year &&
                a.CheckInTime.Month == month)
            .OrderBy(a => a.CheckInTime)
            .ToListAsync();

    public async Task<AttendanceRecord?> GetLastCheckInAsync(Guid employeeId)
        => await _dbSet
            .Where(a => a.EmployeeId == employeeId)
            .OrderByDescending(a => a.CheckInTime)
            .FirstOrDefaultAsync();


    public async Task<IEnumerable<AttendanceRecord>> GetByEmployeeAndDateRangeAsync(Guid employeeId, DateTime start, DateTime end)
        => await _dbSet
            .Where(a => a.EmployeeId == employeeId &&
                        a.CheckInTime >= start &&
                        a.CheckInTime <= end)
            .OrderBy(a => a.CheckInTime)
            .ToListAsync();
}
