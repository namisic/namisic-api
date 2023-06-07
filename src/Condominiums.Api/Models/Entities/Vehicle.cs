using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Condominiums.Api.Models.Entities;

/// <summary>
/// Represents the information stored from a vehicle.
/// </summary>
public class Vehicle
{
    private string _plateNumber = string.Empty;

    /// <summary>
    /// Indicates the type of vehicle. Possible values are "car" or "motorcycle". Default value is "car".
    /// </summary>
    [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Indicates the license plate number of the vehicle.
    /// </summary>
    [BsonElement("plate_number")]
    public string PlateNumber { get => _plateNumber; set => _plateNumber = value.ToUpperInvariant(); }
}
