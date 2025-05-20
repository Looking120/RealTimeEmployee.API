using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Exceptions;
using RealTimeEmployee.BusinessLogic.Helpers;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.BusinessLogic.Services.Interfaces;
using RealTimeEmployee.DataAccess.Constants;
using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.Enums;
using RealTimeEmployee.DataAccess.Models;
using RealTimeEmployee.DataAccess.Repository.Interfaces;

namespace RealTimeEmployee.BusinessLogic.Services.Implementations;

public class UserManagementService : IUserManagementService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<UserManagementService> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UserManagementService(
        UserManager<AppUser> userManager,
        ILogger<UserManagementService> logger,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
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

    public async Task<bool> HireEmployeeAsync(HireEmployeeRequest request)
    {
        var (user, office) = await ValidateHireRequestAsync(request);

        return await ExecuteInTransactionAsync(async () =>
        {
            await UpdateUserRoleToEmployeeAsync(user);
            var employee = await CreateEmployeeAsync(user, request, office);
            await LinkEmployeeToUserAsync(user, employee.Id);

            return true;
        });
    }

    private async Task<(AppUser User, Office Office)> ValidateHireRequestAsync(HireEmployeeRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
            throw new NotFoundException($"User with ID {request.UserId} not found");

        var userRoles = await _userManager.GetRolesAsync(user);
        if (userRoles.Contains(Roles.Employee))
            throw new AlreadyExistsException($"User is already an employee");

        var departmentExists = await _unitOfWork.GetRepository<Department>().ExistsAsync(d => d.Id == request.DepartmentId);
        if (!departmentExists)
            throw new NotFoundException($"Department with ID {request.DepartmentId} not found");

        var positionExists = await _unitOfWork.GetRepository<Position>().ExistsAsync(p => p.Id == request.PositionId);
        if (!positionExists)
            throw new NotFoundException($"Position with ID {request.PositionId} not found");

        var office = await _unitOfWork.GetRepository<Office>().GetByIdAsync(request.OfficeId);
        if (office is null)
            throw new NotFoundException($"Office with ID {request.OfficeId} not found");

        return (user, office);
    }

    private async Task UpdateUserRoleToEmployeeAsync(AppUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        if (userRoles.Any())
        {
            var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
            if (!removeResult.Succeeded)
            {
                removeResult.ThrowExceptionIfResultDoNotSucceed(_logger);
                throw new InvalidOperationException("Failed to remove existing roles");
            }
        }

        var addRoleResult = await _userManager.AddToRoleAsync(user, Roles.Employee);
        if (!addRoleResult.Succeeded)
        {
            addRoleResult.ThrowExceptionIfResultDoNotSucceed(_logger);
            throw new InvalidOperationException("Failed to add Employee role");
        }
    }

    private async Task<Employee> CreateEmployeeAsync(AppUser user, HireEmployeeRequest request, Office office)
    {
        var employee = _mapper.Map<Employee>((user, request, office));

        await _unitOfWork.Employees.AddAsync(employee);
        await _unitOfWork.SaveChangesAsync();

        return employee;
    }

    private async Task LinkEmployeeToUserAsync(AppUser user, Guid employeeId)
    {
        user.EmployeeId = employeeId;
        var updateUserResult = await _userManager.UpdateAsync(user);
        if (!updateUserResult.Succeeded)
        {
            updateUserResult.ThrowExceptionIfResultDoNotSucceed(_logger);
            throw new InvalidOperationException("Failed to update user with employee ID");
        }
    }

    private async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var result = await operation();
            await _unitOfWork.CommitTransactionAsync();
            return result;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Transaction failed: {Message}", ex.Message);
            throw;
        }
    }
}