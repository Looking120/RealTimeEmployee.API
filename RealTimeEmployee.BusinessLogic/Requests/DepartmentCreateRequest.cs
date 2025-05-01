namespace RealTimeEmployee.BusinessLogic.Requests;

public record DepartmentCreateRequest(string Name, string? Description = null);