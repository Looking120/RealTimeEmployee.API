using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Exceptions;
using RealTimeEmployee.BusinessLogic.Helpers;
using RealTimeEmployee.BusinessLogic.Services.Interfaces;
using RealTimeEmployee.DataAccess.Constants;
using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.Models;

namespace RealTimeEmployee.BusinessLogic.Services.Implementations;

public class UserManagementService : IUserManagementService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<UserManagementService> _logger;
    private readonly IMapper _mapper;

    public UserManagementService(
        UserManager<AppUser> userManager,
        ILogger<UserManagementService> logger,
        IMapper mapper)
    {
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<UserDto>> GetAllUsersAsync(PaginationRequest pagination)
    {
        var totalCount = await _userManager.Users.CountAsync();

        var users = await _userManager.Users
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var userDto = _mapper.Map<UserDto>(user);
            var roles = await _userManager.GetRolesAsync(user);

            userDto = userDto with { Roles = roles };

            userDtos.Add(userDto);
        }

        return new PaginatedResult<UserDto>(userDtos, totalCount, pagination.PageNumber, pagination.PageSize);
    }

    public async Task<UserDto> GetUserByIdAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new NotFoundException($"User with ID {userId} not found");

        var userDto = _mapper.Map<UserDto>(user);
        var roles = await _userManager.GetRolesAsync(user);

        return userDto with { Roles = roles };
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new NotFoundException($"User with ID {userId} not found");

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            result.ThrowExceptionIfResultDoNotSucceed(_logger);
            return false;
        }

        return true;
    }

    public async Task<bool> UpdateUserRoleAsync(Guid userId, string roleName)
    {
        if (roleName != Roles.Admin && roleName != Roles.User)
            throw new ValidationException("Invalid role name");

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new NotFoundException($"User with ID {userId} not found");

        var currentRoles = await _userManager.GetRolesAsync(user);

        if (currentRoles.Any())
        {
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                removeResult.ThrowExceptionIfResultDoNotSucceed(_logger);
                return false;
            }
        }

        var addResult = await _userManager.AddToRoleAsync(user, roleName);
        if (!addResult.Succeeded)
        {
            addResult.ThrowExceptionIfResultDoNotSucceed(_logger);
            return false;
        }

        return true;
    }
}