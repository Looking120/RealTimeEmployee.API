using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RealTimeEmployee.DataAccess.Entitites;

namespace RealTimeEmployee.DataAccess.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.EmployeeNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.HireDate)
            .IsRequired();

        // Relationships
        builder.HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Position)
            .WithMany(p => p.Employees)
            .HasForeignKey(e => e.PositionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Manager)
            .WithMany(m => m.Subordinates)
            .HasForeignKey(e => e.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.UserAccount)
            .WithOne(u => u.Employee)
            .HasForeignKey<AppUser>(u => u.EmployeeId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.EmployeeNumber)
            .IsUnique();

        builder.HasIndex(e => e.Email)
            .IsUnique();

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}