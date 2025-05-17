using System.Text.Json.Serialization;

namespace RealTimeEmployee.DataAccess.Enums;

/// <summary>
/// Represents attendance statuses for employees
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AttendanceStatus
{
    Present,
    Late,
    HalfDay,
    Absent,
    OnLeave,
    Holiday
}