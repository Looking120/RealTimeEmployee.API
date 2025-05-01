using RealTimeEmployee.DataAccess.Enums;

namespace RealTimeEmployee.BusinessLogic.Dtos;

public record EmployeeDto(
    Guid Id,
    string FirstName,
    string LastName,
    string? MiddleName,
    string EmployeeNumber,
    string Email,
    string PhoneNumber,
    string DepartmentName,
    string PositionTitle,
    ActivityStatus CurrentStatus,
    DateTime? LastStatusChange);