using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.DataAccess.Enums;
using RealTimeEmployee.DataAccess.Models;

namespace RealTimeEmployee.BusinessLogic.Services.Interfaces;

public interface IEmployeeService
{
    Task<EmployeeDto> GetEmployeeByIdAsync(Guid id);

    Task<PaginatedResult<EmployeeDto>> GetAllEmployeesAsync(PaginationRequest pagination);

    Task<PaginatedResult<EmployeeDto>> GetEmployeesByStatusAsync(ActivityStatus status, PaginationRequest pagination);

    Task UpdateEmployeeStatusAsync(Guid employeeId, ActivityStatus status);

    Task<EmployeeLocationDto> GetCurrentEmployeeLocationAsync(Guid employeeId);

    Task<PaginatedResult<EmployeeDto>> GetEmployeesInLocationRadiusAsync(
        Guid employeeId,
        LocationRadiusRequest request,
        PaginationRequest pagination);
}