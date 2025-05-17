using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.BusinessLogic.Services.Interfaces;

namespace RealTimeEmployee.API.Controllers;

[Route("api/attendance")]
[Authorize]
public class AttendanceController : BaseApiController
{
    private readonly IAttendanceService _attendanceService;

    public AttendanceController(IAttendanceService attendanceService)
    {
        _attendanceService = attendanceService;
    }

    [HttpPost("{employeeId}/check-in")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CheckIn(Guid employeeId)
    {
        await _attendanceService.CheckInAsync(employeeId);
        return NoContent();
    }

    [HttpPost("{employeeId}/check-out")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CheckOut(Guid employeeId)
    {
        await _attendanceService.CheckOutAsync(employeeId);
        return NoContent();
    }

    [HttpGet("{employeeId}/report")]
    [ProducesResponseType(typeof(AttendanceReportDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAttendanceReport(
        Guid employeeId,
        [FromQuery] AttendanceReportRequest request)
    {
        var report = await _attendanceService.GetAttendanceReportAsync(employeeId, request);
        return Ok(report);
    }
}