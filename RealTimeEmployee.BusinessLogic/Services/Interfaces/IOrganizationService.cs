using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Requests;

namespace RealTimeEmployee.BusinessLogic.Services.Interfaces;

public interface IOrganizationService
{
    Task<DepartmentDto> CreateDepartmentAsync(DepartmentCreateRequest request);

    Task<PositionDto> CreatePositionAsync(PositionCreateRequest request);

    Task AssignPositionToDepartmentAsync(Guid positionId, Guid departmentId);

    Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync();

    Task<IEnumerable<PositionDto>> GetAllPositionsAsync();
}