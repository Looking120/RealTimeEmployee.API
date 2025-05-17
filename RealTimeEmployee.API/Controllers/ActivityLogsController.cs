using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.BusinessLogic.Services.Interfaces;
using RealTimeEmployee.DataAccess.Enums;
using RealTimeEmployee.DataAccess.Models;

namespace RealTimeEmployee.API.Controllers;

[Route("api/activity-logs")]
[Authorize]
public class ActivityLogsController : BaseApiController
{
    private readonly IActivityLogService _activityLogService;

    public ActivityLogsController(IActivityLogService activityLogService)
    {
        _activityLogService = activityLogService;
    }

    [HttpPost("{employeeId}/activities")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> LogActivity(Guid employeeId,
        [FromBody] ActivityType activityTypeRequest,
        string Description)
    {
        await _activityLogService.LogActivityAsync(employeeId, activityTypeRequest, Description);
        return NoContent();
    }

    [HttpGet("{employeeId}")]
    [ProducesResponseType(typeof(IEnumerable<ActivityLogDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployeeActivities(
        Guid employeeId,
        [FromQuery] ActivityTimeRangeRequest request,
        [FromQuery] PaginationRequest pagination)
    {
        var result = await _activityLogService.GetEmployeeActivitiesAsync(employeeId, request, pagination);

        AddPaginationHeaders(result);

        return Ok(result.Items);
    }

    [HttpPost("{employeeId}/end-current-activity")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> EndCurrentActivity(Guid employeeId)
    {
        await _activityLogService.EndCurrentActivityAsync(employeeId);
        return NoContent();
    }
}