using RealTimeEmployee.DataAccess.Enums;

namespace RealTimeEmployee.BusinessLogic.Dtos;

public record AttendanceReportDto(
    Guid EmployeeId,
    string EmployeeName,
    DateTime PeriodStart,
    DateTime PeriodEnd,
    int TotalWorkingDays,
    int PresentDays,
    int LateDays,
    int AbsentDays,
    IEnumerable<DailyAttendanceDto> DailyRecords);

public record DailyAttendanceDto(
    DateTime Date,
    DateTime? CheckInTime,
    DateTime? CheckOutTime,
    AttendanceStatus Status);