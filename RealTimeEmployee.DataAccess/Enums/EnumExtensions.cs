using RealTimeEmployee.DataAccess.Enums;

namespace RealTimeEmployee.DataAccess.Extensions;

public static class EnumExtensions
{
    public static ActivityType ToActivityType(this ActivityStatus status)
    {
        return status switch
        {
            ActivityStatus.Online => ActivityType.SystemLogin,
            ActivityStatus.Offline => ActivityType.SystemLogout,
            ActivityStatus.OnBreak => ActivityType.BreakStart,
            ActivityStatus.InMeeting => ActivityType.MeetingStart,
            ActivityStatus.OnTask => ActivityType.TaskStart,
            ActivityStatus.OnLeave => ActivityType.CheckOut,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }

    public static ActivityStatus ToActivityStatus(this ActivityType type)
    {
        return type switch
        {
            ActivityType.SystemLogin => ActivityStatus.Online,
            ActivityType.SystemLogout => ActivityStatus.Offline,
            ActivityType.BreakStart => ActivityStatus.OnBreak,
            ActivityType.BreakEnd => ActivityStatus.Online,
            ActivityType.MeetingStart => ActivityStatus.InMeeting,
            ActivityType.MeetingEnd => ActivityStatus.Online,
            ActivityType.TaskStart => ActivityStatus.OnTask,
            ActivityType.TaskEnd => ActivityStatus.Online,
            ActivityType.CheckIn => ActivityStatus.Online,
            ActivityType.CheckOut => ActivityStatus.Offline,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}