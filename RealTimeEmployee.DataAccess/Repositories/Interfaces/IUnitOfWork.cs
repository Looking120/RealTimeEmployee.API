using RealTimeEmployee.DataAccess.Repositories.Interfaces;

namespace RealTimeEmployee.DataAccess.Repository.Interfaces
{
    /// <summary>
    /// Unit of Work pattern interface for managing transactions
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;

        IEmployeeRepository Employees { get; }

        IActivityLogRepository ActivityLogs { get; }

        ILocationHistoryRepository LocationHistories { get; }

        IAttendanceRecordRepository AttendanceRecords { get; }

        IMessageRepository Messages { get; }

        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();

        Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> operation);

        Task<int> SaveChangesAsync();
    }
}