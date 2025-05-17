namespace RealTimeEmployee.BusinessLogic.Dtos;

public record UserDto(
    Guid Id,
    string UserName,
    string Email,
    string FirstName,
    string LastName,
    string? MiddleName,
    IEnumerable<string> Roles);
