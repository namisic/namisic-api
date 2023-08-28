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
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The resident's document type.
    /// </summary>
    [BsonElement("document_type")]
    [BsonIgnoreIfNull]
    public string? DocumentType { get; set; }

    /// <summary>
    /// The resident's document number.
    /// </summary>
    [BsonElement("document_number")]
    [BsonIgnoreIfNull]
    public string? DocumentNumber { get; set; }

    /// <summary>
    /// The resident's e-mail.
    /// </summary>
    [BsonElement("email")]
    [BsonIgnoreIfNull]
    public string? Email { get; set; }

    /// <summary>
    /// The resident's cellphone.
    /// </summary>
    [BsonElement("cellphone")]
    [BsonIgnoreIfNull]
    public string? Cellphone { get; set; }

    /// <summary>
    /// The resident type. Could be "owner", "tenant" or "resident".
    /// </summary>
    [BsonElement("resident_type")]
    [BsonIgnoreIfNull]
    public string? ResidentType { get; set; }

    /// <summary>
    /// House or apartment number where the resident lives.
    /// </summary>
    [BsonElement("apartment_number")]
    public string ApartmentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Vehicles owned by the resident.
    /// </summary>
    [BsonElement("vehicles")]
    [BsonIgnoreIfNull]
    public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
