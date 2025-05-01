using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.DataAccess.Enums;

namespace RealTimeEmployee.BusinessLogic.Services.Interfaces;

public interface IEmployeeService
{
    Task<EmployeeDto> GetEmployeeByIdAsync(Guid id);

    Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();

    Task<IEnumerable<EmployeeDto>> GetEmployeesByStatusAsync(ActivityStatus status);

    Task UpdateEmployeeStatusAsync(Guid employeeId, ActivityStatus status);

    Task<EmployeeLocationDto> GetCurrentEmployeeLocationAsync(Guid employeeId);

    Task<IEnumerable<EmployeeDto>> GetEmployeesInLocationRadiusAsync(Guid employeeId, LocationRadiusRequest request);
}
