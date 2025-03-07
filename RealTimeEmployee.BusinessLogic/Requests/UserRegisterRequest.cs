namespace RealTimeEmployee.BusinessLogic.Requests;

public class UserRegisterRequest
{
    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public string? MiddleName { get; init; }

    public DateTime BirthDate { get; init; }

    public required string UserName { get; init; }

    public required string Email { get; init; }

    public required string Password { get; init; }

    public required string ConfirmPassword { get; init; }
}