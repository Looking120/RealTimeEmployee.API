namespace RealTimeEmployee.BusinessLogic.Dtos;

public record PositionDto(
    Guid Id,
    string Title,
    Guid DepartmentId,
    string DepartmentName,
    string? Description,
    decimal? BaseSalary,
    DateTime CreatedAt);