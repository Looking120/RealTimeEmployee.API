namespace RealTimeEmployee.BusinessLogic.Dtos;

public record DepartmentDto(
    Guid Id,
    string Name,
    string? Description,
    DateTime CreatedAt);