namespace RealTimeEmployee.BusinessLogic.Dtos;

public record OfficeDto(
    Guid Id,
    string Name,
    string Address,
    double Latitude,
    double Longitude,
    double Radius,
    string? Description);