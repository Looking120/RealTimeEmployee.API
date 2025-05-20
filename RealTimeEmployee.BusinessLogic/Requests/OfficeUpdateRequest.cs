namespace RealTimeEmployee.BusinessLogic.Requests;

public class OfficeUpdateRequest
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public double? Radius { get; set; }
    public string? Description { get; set; }
}