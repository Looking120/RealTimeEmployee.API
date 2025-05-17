using System.Text.Json.Serialization;

namespace RealTimeEmployee.DataAccess.Enums;

/// <summary>
/// Represents types of work locations
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LocationType
{
    Office,
    Remote,
    ClientSite,
    Traveling
}