using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RealTimeEmployee.DataAccess.Entitites;

namespace RealTimeEmployee.DataAccess.Configurations;

public class ActivityLogConfiguration : IEntityTypeConfiguration<ActivityLog>
{
    public void Configure(EntityTypeBuilder<ActivityLog> builder)
    {
        builder.HasKey(a => a.Id);

        // Relationships
        builder.HasOne(a => a.Employee)
            .WithMany(e => e.ActivityLogs)
            .HasForeignKey(a => a.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(a => !a.IsDeleted);
    }
}