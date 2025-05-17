using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.Models;

namespace RealTimeEmployee.BusinessLogic.Services.Interfaces;

public interface IUserManagementService
{
    Task<PaginatedResult<UserDto>> GetAllUsersAsync(PaginationRequest pagination);
    Task<UserDto> GetUserByIdAsync(Guid userId);
    Task<bool> DeleteUserAsync(Guid userId);
    Task<bool> UpdateUserRoleAsync(Guid userId, string roleName);
}
