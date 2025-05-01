using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RealTimeEmployee.DataAccess.Entitites;

namespace RealTimeEmployee.DataAccess.Configurations;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.BaseSalary)
            .HasColumnType("decimal(18,2)");

        builder.HasOne(p => p.Department)
            .WithMany(d => d.Positions)
            .HasForeignKey(p => p.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => p.Title)
            .IsUnique();

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}