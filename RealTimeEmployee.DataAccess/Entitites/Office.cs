using NetTopologySuite.Geometries;

namespace RealTimeEmployee.DataAccess.Entitites;

public class Office : BaseEntity
{
    public required string Name { get; set; }
    public required string Address { get; set; }
    public Point Center { get; set; } = null!;
    public double Radius { get; set; }
    public string? Description { get; set; }
}