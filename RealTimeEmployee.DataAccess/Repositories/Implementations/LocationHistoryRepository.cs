using Microsoft.EntityFrameworkCore;
using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.Repositories.Interfaces;

namespace RealTimeEmployee.DataAccess.Repositories.Implementations;

public class LocationHistoryRepository : Repository<LocationHistory>, ILocationHistoryRepository
{
    public LocationHistoryRepository(DbContext context) : base(context) { }

    public async Task<LocationHistory?> GetLatestAsync(Guid employeeId)
        => await _dbSet
            .Where(l => l.EmployeeId == employeeId)
            .OrderByDescending(l => l.Timestamp)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<LocationHistory>> GetInAreaAsync(
        double minLat,
        double maxLat,
        double minLng,
        double maxLng)
        => await _dbSet
            .Where(l =>
                l.Latitude >= minLat &&
                l.Latitude <= maxLat &&
                l.Longitude >= minLng &&
                l.Longitude <= maxLng)
            .ToListAsync();

    public async Task<IEnumerable<LocationHistory>> GetWithinRadiusAsync(
        double centerLat,
        double centerLng,
        double radiusKm)
    {
        return await _dbSet
            .Where(l =>
                Math.Abs(l.Latitude - centerLat) < radiusKm / 111.32 &&  // ~111.32 km per degree latitude
                Math.Abs(l.Longitude - centerLng) < radiusKm / 111.32)
            .ToListAsync();
    }
}
