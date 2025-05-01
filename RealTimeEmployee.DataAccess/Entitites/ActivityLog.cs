using RealTimeEmployee.DataAccess.Enums;

namespace RealTimeEmployee.DataAccess.Entitites;

public class ActivityLog : BaseEntity
{
    public ActivityType ActivityType { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string? Description { get; set; }

    public Guid EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;
}