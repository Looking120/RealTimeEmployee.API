using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RealTimeEmployee.DataAccess.Data;
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
        private readonly RealTimeEmployeeDbContext _context;
        private IDbContextTransaction? _transaction;

        public IEmployeeRepository Employees { get; }
        public IActivityLogRepository ActivityLogs { get; }
        public ILocationHistoryRepository LocationHistories { get; }
        public IMessageRepository Messages { get; }
        public IAttendanceRecordRepository AttendanceRecords { get; }

        public UnitOfWork(RealTimeEmployeeDbContext context)
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

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync();
                }
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> operation)
        {
            await BeginTransactionAsync();
            try
            {
                var result = await operation();
                await CommitTransactionAsync();
                return result;
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
        }
    }
}