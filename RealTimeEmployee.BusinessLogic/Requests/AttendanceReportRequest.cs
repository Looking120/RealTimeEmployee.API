namespace RealTimeEmployee.BusinessLogic.Requests;

public record AttendanceReportRequest(
    DateTime StartDate,
    DateTime EndDate);
