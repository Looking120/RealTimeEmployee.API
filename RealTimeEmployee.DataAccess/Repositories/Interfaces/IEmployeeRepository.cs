using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.Enums;

namespace RealTimeEmployee.DataAccess.Repositories.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>
{
    /// <summary>
    /// Get employees by status (Online, Offline, etc.)
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    Task<IEnumerable<Employee>> GetByStatusAsync(ActivityStatus status);

    /// <summary>
    /// Get employees in a specific location radius
    /// </summary>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    /// <param name="radiusKm"></param>
    /// <returns></returns>
    Task<IEnumerable<Employee>> GetNearLocationAsync(double latitude, double longitude, double radiusKm);

    /// <summary>
    /// Get employee with details (Department, Position, etc.)
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns></returns>
    Task<Employee?> GetWithDetailsAsync(Guid employeeId);
}