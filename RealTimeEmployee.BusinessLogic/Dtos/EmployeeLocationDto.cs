using RealTimeEmployee.DataAccess.Enums;

namespace RealTimeEmployee.BusinessLogic.Dtos;

public record EmployeeLocationDto(
    Guid EmployeeId,
    string EmployeeName,
    double Latitude,
    double Longitude,
    LocationType LocationType,
    DateTime Timestamp);