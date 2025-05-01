using RealTimeEmployee.DataAccess.Enums;

namespace RealTimeEmployee.DataAccess.Entitites;

public class LocationHistory : BaseEntity
{
    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public DateTime Timestamp { get; set; }

    // Office, Remote, etc.
    public LocationType LocationType { get; set; }

    public Guid EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;
}
