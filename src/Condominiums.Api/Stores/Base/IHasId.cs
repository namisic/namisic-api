using MongoDB.Bson;

namespace Condominiums.Api.Stores.Base;

/// <summary>
/// Defines the Id field to an entity.
/// </summary>
public interface IHasId
{
    /// <summary>
    /// The entity's identifier.
    /// </summary>
    public ObjectId Id { get; set; }
}
