namespace RealTimeEmployee.BusinessLogic.Requests;

public class OfficeCreateRequest
{
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required double Latitude { get; set; }
    public required double Longitude { get; set; }
    public required double Radius { get; set; }
    public string? Description { get; set; }
}