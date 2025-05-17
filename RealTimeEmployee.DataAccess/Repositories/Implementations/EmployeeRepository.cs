using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using RealTimeEmployee.DataAccess.Data;
using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.Enums;
using RealTimeEmployee.DataAccess.Repositories.Interfaces;

namespace RealTimeEmployee.DataAccess.Repositories.Implementations;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(RealTimeEmployeeDbContext context) : base(context) { }

    public async Task<IEnumerable<Employee>> GetByStatusAsync(ActivityStatus status)
        => await _dbSet.Where(e => e.CurrentStatus == status).ToListAsync();

    public async Task<IEnumerable<Employee>> GetNearLocationAsync(double latitude, double longitude, double radiusKm)
    {
        var centerPoint = new Point(longitude, latitude) { SRID = 4326 };
        var radiusMeters = radiusKm * 1000;

        return await _dbSet
            .Where(e => e.Location != null && e.Location.Distance(centerPoint) <= radiusMeters)
            .ToListAsync();
    }

    public async Task<Employee?> GetWithDetailsAsync(Guid employeeId)
        => await _dbSet
            .AsNoTracking()
            .Include(e => e.Department)
            .Include(e => e.Position)
            .Include(e => e.LocationHistories)
            .FirstOrDefaultAsync(e => e.Id == employeeId);
}