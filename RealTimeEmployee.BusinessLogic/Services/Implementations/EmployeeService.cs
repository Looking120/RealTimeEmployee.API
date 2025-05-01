using AutoMapper;
using FluentValidation;
using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Exceptions;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.BusinessLogic.Services.Interfaces;
using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.Enums;
using RealTimeEmployee.DataAccess.Extensions;
using RealTimeEmployee.DataAccess.Repository.Interfaces;

namespace RealTimeEmployee.BusinessLogic.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<LocationRadiusRequest> _locationValidator;

    public EmployeeService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<LocationRadiusRequest> locationValidator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _locationValidator = locationValidator;
    }

    public async Task<EmployeeDto> GetEmployeeByIdAsync(Guid id)
    {
        var employee = await _unitOfWork.Employees.GetWithDetailsAsync(id);
        if (employee == null)
            throw new NotFoundException($"Employee with id {id} not found");

        return _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
    {
        var employees = await _unitOfWork.Employees.GetAllAsync();

        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }

    public async Task<IEnumerable<EmployeeDto>> GetEmployeesByStatusAsync(ActivityStatus status)
    {
        var employees = await _unitOfWork.Employees.GetByStatusAsync(status);

        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }

    public async Task UpdateEmployeeStatusAsync(Guid employeeId, ActivityStatus status)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId);

        if (employee == null)
            throw new NotFoundException($"Employee with ID {employeeId} not found");

        employee.CurrentStatus = status;
        employee.LastStatusChange = DateTime.UtcNow;

        await _unitOfWork.ActivityLogs.AddAsync(new ActivityLog
        {
            EmployeeId = employeeId,
            ActivityType = status.ToActivityType(),
            StartTime = DateTime.UtcNow
        });

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<EmployeeLocationDto> GetCurrentEmployeeLocationAsync(Guid employeeId)
    {
        var location = await _unitOfWork.LocationHistories.GetLatestAsync(employeeId);

        if (location == null)
            throw new NotFoundException($"No location found for employee {employeeId}");

        var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId);

        return new EmployeeLocationDto(
            employeeId,
            $"{employee.FirstName} {employee.LastName}",
            location.Latitude,
            location.Longitude,
            location.LocationType,
            location.Timestamp);
    }

    public async Task<IEnumerable<EmployeeDto>> GetEmployeesInLocationRadiusAsync(
        Guid employeeId,
        LocationRadiusRequest request)
    {
        await _locationValidator.ValidateAndThrowAsync(request);

        var employees = await _unitOfWork.Employees.GetNearLocationAsync(
            request.Latitude,
            request.Longitude,
            request.RadiusKm);

        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }
}
