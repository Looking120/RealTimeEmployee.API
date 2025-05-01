using RealTimeEmployee.DataAccess.Enums;

namespace RealTimeEmployee.DataAccess.Entitites;

public class AttendanceRecord : BaseEntity
{
    public DateTime CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    // Present, Late, Absent, etc.
    public AttendanceStatus Status { get; set; } 

    public Guid EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;
}
