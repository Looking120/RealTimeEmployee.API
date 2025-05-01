namespace RealTimeEmployee.DataAccess.Enums;

/// <summary>
/// Represents types of activities logged in the system
/// </summary>
public enum ActivityType
{
    CheckIn,
    CheckOut,
    BreakStart,
    BreakEnd,
    TaskStart,
    TaskEnd,
    MeetingStart,
    MeetingEnd,
    SystemLogin,
    SystemLogout
}