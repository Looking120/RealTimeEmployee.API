using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.Configurations;

namespace RealTimeEmployee.DataAccess.Data;

public class RealTimeEmployeeDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
    public RealTimeEmployeeDbContext(DbContextOptions<RealTimeEmployeeDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(IdentityUserConfiguration).Assembly);
    }
}