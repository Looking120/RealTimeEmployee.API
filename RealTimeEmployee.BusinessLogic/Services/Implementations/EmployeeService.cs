using AutoMapper;
using FluentValidation;
using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Exceptions;
using RealTimeEmployee.BusinessLogic.Profiles;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.BusinessLogic.Services.Interfaces;
using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.Enums;
using RealTimeEmployee.DataAccess.Extensions;
using RealTimeEmployee.DataAccess.Models;
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

    public async Task<PaginatedResult<EmployeeDto>> GetAllEmployeesAsync(PaginationRequest pagination)
    {
        var result = await _unitOfWork.Employees.GetPagedAsync(pagination);

        return result.ToPaginatedResult<Employee, EmployeeDto>(_mapper);
    }

    public async Task<PaginatedResult<EmployeeDto>> GetEmployeesByStatusAsync(
        ActivityStatus status,
        PaginationRequest pagination)
    {
        var result = await _unitOfWork.Employees.GetPagedAsync(
            pagination,
            e => e.CurrentStatus == status);

        return result.ToPaginatedResult<Employee, EmployeeDto>(_mapper);
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

        var locationDto = _mapper.Map<EmployeeLocationDto>(location);

        return locationDto with { EmployeeName = $"{employee.FirstName} {employee.LastName}" };
    }

    public async Task<PaginatedResult<EmployeeDto>> GetEmployeesInLocationRadiusAsync(
        Guid employeeId,
        LocationRadiusRequest request,
        PaginationRequest pagination)
    {
        await _locationValidator.ValidateAndThrowAsync(request);

        var employees = await _unitOfWork.Employees.GetNearLocationAsync(
            request.Latitude,
            request.Longitude,
            request.RadiusKm);

        var totalCount = employees.Count();

        var pagedEmployees = employees
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize);

        var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(pagedEmployees);

        return new PaginatedResult<EmployeeDto>(
            employeeDtos,
            totalCount,
            pagination.PageNumber,
            pagination.PageSize);
    }
}