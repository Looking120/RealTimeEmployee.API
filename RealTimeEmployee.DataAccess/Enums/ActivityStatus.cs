using System.Text.Json.Serialization;

namespace RealTimeEmployee.DataAccess.Enums;

/// <summary>
/// Represents the current activity status of an employee
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ActivityStatus
{
    Offline = 0,
    Online = 1,
    Busy = 2,
    Away = 3,
    OnBreak = 4,
    OnTask = 5,
    InMeeting = 6,
    DoNotDisturb = 7,
    Available = 8,
    OnLeave = 9
}