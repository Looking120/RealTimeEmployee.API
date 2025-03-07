using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Requests;

namespace RealTimeEmployee.BusinessLogic.Services.Interfaces;

public interface IAuthService
{
    Task<TokenDto> SignInAsync(UserSignInRequest request, CancellationToken cancellationToken);
    Task<TokenDto> SignUpAsync(UserRegisterRequest request, CancellationToken cancellationToken);
    Task SignOutAsync(Guid userId);
}