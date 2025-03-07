using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.DataAccess.Entitites;

namespace RealTimeEmployee.BusinessLogic.Services.Interfaces;

public interface IUserService
{
    Task<TokenDto> GenerateTokenAsync(AppUser user);

    Task<AppUser> GetUserByEmail(string email);

    Task CheckPassword(AppUser user, string password);

    Task EnsureEmailConfirmed(AppUser user);

    Task ValidateRequest<T>(T request, CancellationToken cancellationToken) where T : class;
}