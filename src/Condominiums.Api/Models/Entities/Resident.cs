using Condominiums.Api.Models.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Condominiums.Api.Models.Entities;

/// <summary>
/// Represents the information stored from a resident.
/// </summary>
public class Resident : IHasId
{
    public ObjectId Id { get; set; }

    /// <summary>
    /// The resident's name.
    /// </summary>
    [BsonElement("name")]
    public string Name { get; set; } = String.Empty;

    /// <summary>
    /// House or apartment number where the resident lives.
    /// </summary>
    [BsonElement("apartment_number")]
    public string ApartmentNumber { get; set; } = String.Empty;

    /// <summary>
    /// Vehicles owned by the resident.
    /// </summary>
    [BsonElement("vehicles")]
    [BsonIgnoreIfNull]
    public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
