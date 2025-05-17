using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.Models;

namespace RealTimeEmployee.BusinessLogic.Services.Interfaces;

public interface ILocationService
{
    Task UpdateEmployeeLocationAsync(Guid employeeId, UpdateLocationRequest request);

    Task<EmployeeLocationDto> GetEmployeeLocationHistoryAsync(Guid employeeId, int hoursBack = 24);

    Task<PaginatedResult<EmployeeLocationDto>> GetEmployeeLocationHistoryAsync(Guid employeeId, int hoursBack, PaginationRequest pagination);
    
    Task<bool> IsInOfficeRadius(Guid employeeId, Office office);
}