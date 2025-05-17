using NetTopologySuite.Geometries;
using RealTimeEmployee.DataAccess.Enums;

namespace RealTimeEmployee.DataAccess.Entitites;
public class Employee : BaseEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? MiddleName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public required string EmployeeNumber { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
    public ActivityStatus CurrentStatus { get; set; }
    public DateTime? LastStatusChange { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Point? Location { get; set; }

    public Guid DepartmentId { get; set; }
    public Guid PositionId { get; set; }
    public Guid? ManagerId { get; set; }

    // Navigation properties
    public Department Department { get; set; } = null!;
    public Position Position { get; set; } = null!;
    public Employee? Manager { get; set; }
    public ICollection<Employee> Subordinates { get; set; } = [];
    public ICollection<LocationHistory> LocationHistories { get; set; } = [];
    public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = [];
    public ICollection<ActivityLog> ActivityLogs { get; set; } = [];
    public ICollection<Message> SentMessages { get; set; } = [];
    public ICollection<Message> ReceivedMessages { get; set; } = [];
    public AppUser? UserAccount { get; set; }
}