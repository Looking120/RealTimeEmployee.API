using System.Text.Json.Serialization;

namespace RealTimeEmployee.DataAccess.Enums;

/// <summary>
/// Represents the current activity status of an employee
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ActivityStatus
{
    Offline,
    Online,
    OnBreak,
    InMeeting,
    OnTask,
    OnLeave
}