namespace RealTimeEmployee.BusinessLogic.Requests;

public record LocationRadiusRequest(
    double Latitude,
    double Longitude,
    double RadiusKm);
