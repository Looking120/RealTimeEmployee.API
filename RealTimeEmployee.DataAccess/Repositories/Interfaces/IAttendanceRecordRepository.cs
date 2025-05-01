using RealTimeEmployee.DataAccess.Entitites;

namespace RealTimeEmployee.DataAccess.Repositories.Interfaces;

public interface IAttendanceRecordRepository : IRepository<AttendanceRecord>
{
    Task<IEnumerable<AttendanceRecord>> GetByEmployeeAndMonthAsync(Guid employeeId, int year, int month);

    Task<AttendanceRecord?> GetLastCheckInAsync(Guid employeeId);

    Task<IEnumerable<AttendanceRecord>> GetByEmployeeAndDateRangeAsync(Guid employeeId, DateTime start, DateTime end);
}
