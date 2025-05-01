using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RealTimeEmployee.DataAccess.Entitites;

namespace RealTimeEmployee.DataAccess.Configurations;

public class LocationHistoryConfiguration : IEntityTypeConfiguration<LocationHistory>
{
    public void Configure(EntityTypeBuilder<LocationHistory> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Timestamp)
            .IsRequired();

        // Relationships
        builder.HasOne(l => l.Employee)
            .WithMany(e => e.LocationHistories)
            .HasForeignKey(l => l.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(l => !l.IsDeleted);
    }
}