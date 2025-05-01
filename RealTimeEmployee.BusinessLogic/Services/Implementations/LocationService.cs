using FluentValidation;
using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Exceptions;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.BusinessLogic.Services.Interfaces;
using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.Repository.Interfaces;

namespace RealTimeEmployee.BusinessLogic.Services.Implementations;

public class LocationService : ILocationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateLocationRequest> _locationValidator;

    public LocationService(
        IUnitOfWork unitOfWork,
        IValidator<UpdateLocationRequest> locationValidator)
    {
        _unitOfWork = unitOfWork;
        _locationValidator = locationValidator;
    }

    public async Task UpdateEmployeeLocationAsync(
        Guid employeeId,
        UpdateLocationRequest request)
    {
        await _locationValidator.ValidateAndThrowAsync(request);

        await _unitOfWork.LocationHistories.AddAsync(new LocationHistory
        {
            EmployeeId = employeeId,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            LocationType = request.LocationType,
            Timestamp = DateTime.UtcNow
        });

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<EmployeeLocationDto> GetEmployeeLocationHistoryAsync(
        Guid employeeId,
        int hoursBack = 24)
    {
        var latestLocation = await _unitOfWork.LocationHistories.GetLatestAsync(employeeId);
        if (latestLocation == null)
            throw new NotFoundException($"No location found for employee {employeeId}");

        var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId);

        return new EmployeeLocationDto(
            employeeId,
            $"{employee.FirstName} {employee.LastName}",
            latestLocation.Latitude,
            latestLocation.Longitude,
            latestLocation.LocationType,
            latestLocation.Timestamp);
    }
}