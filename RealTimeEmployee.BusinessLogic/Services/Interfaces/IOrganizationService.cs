using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.DataAccess.Models;

namespace RealTimeEmployee.BusinessLogic.Services.Interfaces;

public interface IOrganizationService
{
    Task<DepartmentDto> CreateDepartmentAsync(DepartmentCreateRequest request);
    Task<PositionDto> CreatePositionAsync(PositionCreateRequest request);
    Task AssignPositionToDepartmentAsync(Guid positionId, Guid departmentId);
    Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync();
    Task<PaginatedResult<DepartmentDto>> GetAllDepartmentsAsync(PaginationRequest pagination);
    Task<IEnumerable<PositionDto>> GetAllPositionsAsync();
    Task<PaginatedResult<PositionDto>> GetAllPositionsAsync(PaginationRequest pagination);
}