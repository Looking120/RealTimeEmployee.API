using Microsoft.EntityFrameworkCore;
using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.Enums;
using RealTimeEmployee.DataAccess.Repositories.Interfaces;

namespace RealTimeEmployee.DataAccess.Repositories.Implementations;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(DbContext context) : base(context) { }

    public async Task<IEnumerable<Employee>> GetByStatusAsync(ActivityStatus status)
        => await _dbSet.Where(e => e.CurrentStatus == status).ToListAsync();

    public async Task<IEnumerable<Employee>> GetNearLocationAsync(double latitude, double longitude, double radiusKm)
    {
        // (Simplified) Query employees within a radius using EF.Functions
        return await _dbSet
            .Where(e => e.LocationHistories.Any(l =>
                Math.Abs(l.Latitude - latitude) < radiusKm / 111.32 && // Approx. km per degree latitude
                Math.Abs(l.Longitude - longitude) < radiusKm / 111.32))
            .ToListAsync();
    }

    public async Task<Employee?> GetWithDetailsAsync(Guid employeeId)
        => await _dbSet
            .Include(e => e.Department)
            .Include(e => e.Position)
            .Include(e => e.LocationHistories)
            .FirstOrDefaultAsync(e => e.Id == employeeId);
}