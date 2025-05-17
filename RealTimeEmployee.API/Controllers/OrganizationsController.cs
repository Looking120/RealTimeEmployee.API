using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.BusinessLogic.Services.Interfaces;
using RealTimeEmployee.DataAccess.Constants;
using RealTimeEmployee.DataAccess.Models;

namespace RealTimeEmployee.API.Controllers;

[Route("api/organization")]
[Authorize(Roles = Roles.Admin)]
public class OrganizationsController : BaseApiController
{
    private readonly IOrganizationService _organizationService;

    public OrganizationsController(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    [HttpPost("departments")]
    [ProducesResponseType(typeof(DepartmentDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateDepartment([FromBody] DepartmentCreateRequest request)
    {
        var department = await _organizationService.CreateDepartmentAsync(request);
        return Ok(department);
    }

    [HttpPost("positions")]
    [ProducesResponseType(typeof(PositionDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreatePosition([FromBody] PositionCreateRequest request)
    {
        var position = await _organizationService.CreatePositionAsync(request);
        return Ok(position);
    }

    [HttpPut("positions/{positionId}/assign")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AssignPositionToDepartment(
        Guid positionId,
        [FromQuery] Guid departmentId)
    {
        await _organizationService.AssignPositionToDepartmentAsync(positionId, departmentId);
        return NoContent();
    }

    [HttpGet("departments")]
    [ProducesResponseType(typeof(IEnumerable<DepartmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDepartments([FromQuery] PaginationRequest pagination)
    {
        var result = await _organizationService.GetAllDepartmentsAsync(pagination);
        AddPaginationHeaders(result);

        return Ok(result.Items);
    }

    [HttpGet("positions")]
    [ProducesResponseType(typeof(IEnumerable<PositionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPositions([FromQuery] PaginationRequest pagination)
    {
        var result = await _organizationService.GetAllPositionsAsync(pagination);
        AddPaginationHeaders(result);

        return Ok(result.Items);
    }
}