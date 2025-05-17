using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.BusinessLogic.Services.Interfaces;
using RealTimeEmployee.DataAccess.Enums;
using RealTimeEmployee.DataAccess.Models;

namespace RealTimeEmployee.API.Controllers;

[Route("api/employees")]
[Authorize]
public class EmployeesController : BaseApiController
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEmployees([FromQuery] PaginationRequest pagination)
    {
        var result = await _employeeService.GetAllEmployeesAsync(pagination);

        AddPaginationHeaders(result);

        return Ok(result.Items);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployeeById(Guid id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        return Ok(employee);
    }

    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployeesByStatus(
        ActivityStatus status,
        [FromQuery] PaginationRequest pagination)
    {
        var result = await _employeeService.GetEmployeesByStatusAsync(status, pagination);

        AddPaginationHeaders(result);

        return Ok(result.Items);
    }

    [HttpPut("{employeeId:guid}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateEmployeeStatus(
        Guid employeeId,
        [FromBody] ActivityStatus status)
    {
        await _employeeService.UpdateEmployeeStatusAsync(employeeId, status);
        return NoContent();
    }

    [HttpGet("{employeeId:guid}/location/current")]
    [ProducesResponseType(typeof(EmployeeLocationDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentEmployeeLocation(Guid employeeId)
    {
        var location = await _employeeService.GetCurrentEmployeeLocationAsync(employeeId);
        return Ok(location);
    }

    [HttpGet("{employeeId:guid}/location/nearby")]
    [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployeesInLocationRadius(
        Guid employeeId,
        [FromQuery] LocationRadiusRequest request,
        [FromQuery] PaginationRequest pagination)
    {
        var result = await _employeeService.GetEmployeesInLocationRadiusAsync(employeeId, request, pagination);

        AddPaginationHeaders(result);

        return Ok(result.Items);
    }
}