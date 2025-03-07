namespace RealTimeEmployee.BusinessLogic.Dtos;

public class TokenDto
{
    public Guid Id { get; init; }

    public required string UserName { get; init; }

    public required string Email { get; init; }

    public required string Role { get; init; }

    public required string AccessToken { get; init; }

    public required int DurationInMinutes { get; init; }
}
