using Condominiums.Api.Models.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Condominiums.Api.Models.Entities;

/// <summary>
/// Represents an entry or exit of vehicle.
/// </summary>
public class VehicleEntryExit : IHasId
{
    public ObjectId Id { get; set; }

    /// <summary>
    /// Indicates the license plate number of the vehicle.
    /// </summary>
    [BsonElement("plate_number")]
    public string PlateNumber { get; set; } = string.Empty;

    /// <summary>
    /// Indicates the type of vehicle. Possible values are "entry" or "exit".
    /// </summary>
    [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// indicates any observation about the event.
    /// </summary>
    [BsonElement("remarks")]
    public string? Remarks { get; set; }

    /// <summary>
    /// Indicates the date when the record was created
    /// </summary>
    [BsonElement("creation_date")]
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Indicates the user who created the record.
    /// </summary>
    [BsonElement("created_by")]
    public string CreatedBy { get; set; } = string.Empty;
}
