namespace RealTimeEmployee.BusinessLogic.Requests;

public record PositionCreateRequest(
    string Title,
    Guid DepartmentId,
    string? Description = null,
    decimal? BaseSalary = null);