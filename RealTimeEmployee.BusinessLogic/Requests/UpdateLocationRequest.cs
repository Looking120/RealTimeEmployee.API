using RealTimeEmployee.DataAccess.Enums;

namespace RealTimeEmployee.BusinessLogic.Requests;

public record UpdateLocationRequest(double Latitude, double Longitude, LocationType LocationType);
