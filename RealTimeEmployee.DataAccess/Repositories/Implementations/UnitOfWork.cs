using Microsoft.EntityFrameworkCore;
using RealTimeEmployee.DataAccess.Repositories.Implementations;
using RealTimeEmployee.DataAccess.Repositories.Interfaces;
using RealTimeEmployee.DataAccess.Repository.Interfaces;

namespace RealTimeEmployee.DataAccess.Repository.Implementations
{
    /// <summary>
    /// Unit of Work implementation for managing transactions
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        public IEmployeeRepository Employees { get; }
        public IActivityLogRepository ActivityLogs { get; }
        public ILocationHistoryRepository LocationHistories { get; }
        public IMessageRepository Messages { get; }
        public IAttendanceRecordRepository AttendanceRecords { get; }

        public UnitOfWork(DbContext context)
        {
            _context = context;

            Employees = new EmployeeRepository(context);
            ActivityLogs = new ActivityLogRepository(context);
            LocationHistories = new LocationHistoryRepository(context);
            Messages = new MessageRepository(context);
            AttendanceRecords = new AttendanceRecordRepository(context);
        }

        public IRepository<T> GetRepository<T>() where T : class
        => new Repository<T>(_context);

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public void Dispose()
            => _context.Dispose();
    }
}