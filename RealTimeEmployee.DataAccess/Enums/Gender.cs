using System.Text.Json.Serialization;

namespace RealTimeEmployee.DataAccess.Enums;

/// <summary>
/// Represents gender options for employees
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Gender
{
    Male,
    Female,
    Other
}