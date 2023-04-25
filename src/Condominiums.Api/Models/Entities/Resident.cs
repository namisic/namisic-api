using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Condominiums.Api.Models.Entities;

/// <summary>
/// Represent the information stored from a resident.
/// </summary>
public class Resident
{
    /// <summary>
    /// The resident's identifier.
    /// </summary>
    public ObjectId Id { get; set; }

    /// <summary>
    /// The resident's name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// House or apartment number where the resident lives.
    /// </summary>
    [BsonElement("apartment_number")]
    public string ApartmentNumber { get; set; } = String.Empty;
}
