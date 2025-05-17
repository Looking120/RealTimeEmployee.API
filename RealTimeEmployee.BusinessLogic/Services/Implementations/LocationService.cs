using AutoMapper;
using FluentValidation;
using NetTopologySuite.Geometries;
using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Exceptions;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.BusinessLogic.Services.Interfaces;
using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.Models;
using RealTimeEmployee.DataAccess.Repository.Interfaces;

namespace RealTimeEmployee.BusinessLogic.Services.Implementations;

public class LocationService : ILocationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateLocationRequest> _locationValidator;
    private readonly IMapper _mapper;

    public LocationService(
        IUnitOfWork unitOfWork,
        IValidator<UpdateLocationRequest> locationValidator,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _locationValidator = locationValidator;
        _mapper = mapper;
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

        return _mapper.Map<EmployeeLocationDto>(latestLocation);
    }

    public async Task<bool> IsInOfficeRadius(Guid employeeId, Office office)
    {
        var location = await _unitOfWork.LocationHistories.GetLatestAsync(employeeId);
        if (location == null) return false;

        return CalculateDistance(location, office.Center) <= office.Radius;
    }

    private double CalculateDistance(LocationHistory location, Point officeCenter)
    {
        // Haversine formula for calculating distance between two points on Earth
        const double earthRadiusKm = 6371.0;

        var lat1 = location.Latitude * Math.PI / 180;
        var lon1 = location.Longitude * Math.PI / 180;
        var lat2 = officeCenter.Y * Math.PI / 180;
        var lon2 = officeCenter.X * Math.PI / 180;

        var dLat = lat2 - lat1;
        var dLon = lon2 - lon1;

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1) * Math.Cos(lat2) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return earthRadiusKm * c;
    }

    public async Task<PaginatedResult<EmployeeLocationDto>> GetEmployeeLocationHistoryAsync(
        Guid employeeId,
        int hoursBack,
        PaginationRequest pagination)
    {
        var startTime = DateTime.UtcNow.AddHours(-hoursBack);

        var locations = await _unitOfWork.LocationHistories.GetByEmployeeAndDateRangeAsync(
            employeeId,
            startTime,
            DateTime.UtcNow);

        var totalCount = locations.Count();

        var pagedLocations = locations
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();

        return new PaginatedResult<EmployeeLocationDto>(
            _mapper.Map<IEnumerable<EmployeeLocationDto>>(pagedLocations),
            totalCount,
            pagination.PageNumber,
            pagination.PageSize);
    }
}