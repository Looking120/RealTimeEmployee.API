using RealTimeEmployee.DataAccess.Entitites;

namespace RealTimeEmployee.DataAccess.Repositories.Interfaces;

public interface ILocationHistoryRepository : IRepository<LocationHistory>
{
    /// <summary>
    /// Get latest location of an employee
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns></returns>
    Task<LocationHistory?> GetLatestAsync(Guid employeeId);

    /// <summary>
    /// Get locations in a geographic area
    /// </summary>
    /// <param name="minLat"></param>
    /// <param name="maxLat"></param>
    /// <param name="minLng"></param>
    /// <param name="maxLng"></param>
    /// <returns></returns>
    Task<IEnumerable<LocationHistory>> GetInAreaAsync(double minLat, double maxLat, double minLng, double maxLng);

    Task<IEnumerable<LocationHistory>> GetByEmployeeAndDateRangeAsync(Guid employeeId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<LocationHistory>> GetWithinRadiusAsync(double centerLat, double centerLng, double radiusKm);
}
