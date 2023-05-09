using Condominiums.Api.Stores.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Condominiums.Api.Models.Entities;

/// <summary>
/// Represents the information stored from a vehicle.
/// </summary>
public class Vehicle : IHasId
{
    /// <summary>
    /// The vehicle's identifier.
    /// </summary>
    [BsonId]
    public ObjectId Id { get; set; }

    /// <summary>
    /// Indicates the type of vehicle. Possible values are "car" or "motorcycle". Default value is "car".
    /// </summary>
    [BsonElement("type")]
    public string Type { get; set; } = "car";

    /// <summary>
    /// Indicates the license plate number of the vehicle.
    /// </summary>
    [BsonElement("plate_number")]
    public string PlateNumber { get; set; } = string.Empty;
}
