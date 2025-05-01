using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Requests;

namespace RealTimeEmployee.BusinessLogic.Services.Interfaces;

public interface IAttendanceService
{
    Task CheckInAsync(Guid employeeId);

    Task CheckOutAsync(Guid employeeId);

    Task<AttendanceReportDto> GetAttendanceReportAsync(Guid employeeId, AttendanceReportRequest request);
}