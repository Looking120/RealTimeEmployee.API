using FluentValidation;
using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Exceptions;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.BusinessLogic.Services.Interfaces;
using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.Enums;
using RealTimeEmployee.DataAccess.Repository.Interfaces;

namespace RealTimeEmployee.BusinessLogic.Services.Implementations;

public class AttendanceService : IAttendanceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IActivityLogService _activityLogService;
    private readonly IValidator<AttendanceReportRequest> _reportValidator;

    public AttendanceService(
        IUnitOfWork unitOfWork,
        IActivityLogService activityLogService,
        IValidator<AttendanceReportRequest> reportValidator)
    {
        _unitOfWork = unitOfWork;
        _activityLogService = activityLogService;
        _reportValidator = reportValidator;
    }

    public async Task CheckInAsync(Guid employeeId)
    {
        var today = DateTime.UtcNow.Date;
        var existingRecord = (await _unitOfWork.AttendanceRecords.GetByEmployeeAndDateRangeAsync(
                employeeId,
                today,
                today.AddDays(1)))
            .FirstOrDefault();

        if (existingRecord != null)
            throw new AlreadyExistsException("Employee has already checked in today");

        await _unitOfWork.AttendanceRecords.AddAsync(new AttendanceRecord
        {
            EmployeeId = employeeId,
            CheckInTime = DateTime.UtcNow,
            Status = DetermineCheckInStatus(DateTime.UtcNow)
        });

        await _activityLogService.LogActivityAsync(
            employeeId,
            ActivityType.CheckIn,
            "Daily check-in");

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task CheckOutAsync(Guid employeeId)
    {
        var today = DateTime.UtcNow.Date;
        var record = (await _unitOfWork.AttendanceRecords.GetByEmployeeAndDateRangeAsync(
                employeeId,
                today,
                today.AddDays(1)))
            .FirstOrDefault();

        if (record == null)
            throw new NotFoundException("No check-in record found for today");

        if (record.CheckOutTime.HasValue)
            throw new AlreadyExistsException("Employee has already checked out today");

        record.CheckOutTime = DateTime.UtcNow;

        await _activityLogService.LogActivityAsync(
            employeeId,
            ActivityType.CheckOut,
            "Daily check-out");

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<AttendanceReportDto> GetAttendanceReportAsync(
        Guid employeeId,
        AttendanceReportRequest request)
    {
        await _reportValidator.ValidateAndThrowAsync(request);

        var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId);

        if (employee == null)
            throw new NotFoundException($"Employee with ID {employeeId} not found");

        var records = await _unitOfWork.AttendanceRecords.GetByEmployeeAndDateRangeAsync(
            employeeId,
            request.StartDate.Date,
            request.EndDate.Date.AddDays(1));

        var dailyRecords = records
            .OrderBy(r => r.CheckInTime)
            .Select(r => new DailyAttendanceDto(
                r.CheckInTime.Date,
                r.CheckInTime,
                r.CheckOutTime,
                r.Status))
            .ToList();

        return new AttendanceReportDto(
            employeeId,
            $"{employee.FirstName} {employee.LastName}",
            request.StartDate,
            request.EndDate,
            (int)(request.EndDate - request.StartDate).TotalDays + 1,
            dailyRecords.Count(r => r.Status == AttendanceStatus.Present),
            dailyRecords.Count(r => r.Status == AttendanceStatus.Late),
            (int)(request.EndDate - request.StartDate).TotalDays + 1 - dailyRecords.Count,
            dailyRecords);
    }

    private AttendanceStatus DetermineCheckInStatus(DateTime checkInTime)
    {
        var expectedStart = checkInTime.Date.AddHours(9); // 9 AM
        return checkInTime > expectedStart.AddMinutes(15) ?
            AttendanceStatus.Late :
            AttendanceStatus.Present;
    }
}