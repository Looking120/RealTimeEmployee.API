using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.BusinessLogic.Services.Interfaces;
using RealTimeEmployee.DataAccess.Models;

namespace RealTimeEmployee.API.Controllers;

[Route("api/location")]
[Authorize]
public class LocationsController : BaseApiController
{
    private readonly ILocationService _locationService;

    public LocationsController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpPut("{employeeId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateLocation(
        Guid employeeId,
        [FromBody] UpdateLocationRequest request)
    {
        await _locationService.UpdateEmployeeLocationAsync(employeeId, request);
        return NoContent();
    }

    [HttpGet("{employeeId}")]
    [ProducesResponseType(typeof(EmployeeLocationDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentLocation(
        Guid employeeId,
        [FromQuery] PaginationRequest pagination,
        [FromQuery] int hoursBack = 24)
    {
        var result = await _locationService.GetEmployeeLocationHistoryAsync(employeeId, hoursBack, pagination);

        AddPaginationHeaders(result);

        return Ok(result.Items);
    }
}