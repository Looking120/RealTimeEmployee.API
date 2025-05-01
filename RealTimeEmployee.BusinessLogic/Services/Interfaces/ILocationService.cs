using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Requests;

namespace RealTimeEmployee.BusinessLogic.Services.Interfaces;

public interface ILocationService
{
    Task UpdateEmployeeLocationAsync(Guid employeeId, UpdateLocationRequest request);

    Task<EmployeeLocationDto> GetEmployeeLocationHistoryAsync(Guid employeeId, int hoursBack = 24);
}
