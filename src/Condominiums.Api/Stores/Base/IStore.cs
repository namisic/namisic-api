using MongoDB.Bson;

namespace Condominiums.Api.Stores.Base;

/// <summary>
/// Defines the general methods to perform the storage of an entity.
/// </summary>
/// <typeparam name="TCollection">The entity type.</typeparam>
public interface IStore<TCollection>
{
    /// <summary>
    /// Allows to get a document of entity type by Id.
    /// </summary>
    /// <param name="id">The entity Id</param>
    /// <returns>The document of entity type.</returns>
    Task<TCollection?> GetByIdAsync(ObjectId id);

    /// <summary>
    /// Allows to insert one document of entity type.
    /// </summary>
    /// <param name="document">The entity type.</param>
    Task InsertOneAsync(TCollection document);
}
