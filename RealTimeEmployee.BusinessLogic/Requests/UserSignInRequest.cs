namespace RealTimeEmployee.BusinessLogic.Requests;

public class UserSignInRequest
{
    public required string Email { get; init; }

    public required string Password { get; init; }
}