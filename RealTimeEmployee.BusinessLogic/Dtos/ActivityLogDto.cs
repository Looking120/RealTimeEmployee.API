using RealTimeEmployee.DataAccess.Enums;

namespace RealTimeEmployee.BusinessLogic.Dtos;

public record ActivityLogDto(
    Guid Id,
    Guid EmployeeId,
    string EmployeeName,
    ActivityType ActivityType,
    DateTime StartTime,
    DateTime? EndTime,
    string? Description);