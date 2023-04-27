using MongoDB.Driver;

namespace Condominiums.Api.Stores.Base;

/// <summary>
/// Defines the general methods to perform the storage of an entity.
/// </summary>
/// <typeparam name="TCollection">The entity type.</typeparam>
public interface IStore<TCollection> where TCollection : IHasId
{
    /// <summary>
    /// Allows to get all documents of entity type.
    /// </summary>
    /// <param name="id">The entity Id</param>
    /// <returns>The document of entity type.</returns>
    Task<List<TCollection>> GetAllAsync(SortDefinition<TCollection>? sort = null);

    /// <summary>
    /// Allows to get a document of entity type by Id.
    /// </summary>
    /// <param name="id">The entity Id</param>
    /// <returns>The document of entity type.</returns>
    Task<TCollection?> GetByIdAsync(string id);

    /// <summary>
    /// Allows to insert one document of entity type.
    /// </summary>
    /// <param name="document">The entity type.</param>
    Task InsertOneAsync(TCollection document);

    /// <summary>
    /// Allows to delete a document of entity type by Id.
    /// </summary>
    /// <param name="id">The entity Id</param>
    /// <returns>Task result.</returns>
    Task DeleteOneAsync(string id);
}
