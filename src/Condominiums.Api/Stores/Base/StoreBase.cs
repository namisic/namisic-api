using MongoDB.Bson;
using MongoDB.Driver;

namespace Condominiums.Api.Stores.Base;

/// <summary>
/// Base IStore implementation.
/// </summary>
/// <typeparam name="TCollection">The entity type.</typeparam>
public class StoreBase<TCollection> : IStore<TCollection>
    where TCollection : IHasId
{
    private readonly string _collectionName;
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<TCollection> _collection;

    public IMongoCollection<TCollection> Collection { get { return _collection; } }

    public StoreBase(string collectionName, IMongoDatabase database)
    {
        _collectionName = collectionName;
        _database = database;
        _collection = database.GetCollection<TCollection>(collectionName);
    }

    /// <summary>
    /// Creates a filter definition for the collection filtering by Id.
    /// </summary>
    /// <param name="id">Id value.</param>
    /// <returns>Filter definition for the collection.</returns>
    public FilterDefinition<TCollection> CreateFilterById(string id)
    {
        ObjectId objectId = ObjectId.Parse(id);
        FilterDefinition<TCollection> filter = Builders<TCollection>.Filter.Eq(r => r.Id, objectId);
        return filter;
    }

    public Task<TCollection?> GetByIdAsync(string id)
    {
        FilterDefinition<TCollection> filter = CreateFilterById(id);
        return _collection.Find(filter).FirstOrDefaultAsync();
    }

    public Task InsertOneAsync(TCollection document) => _collection.InsertOneAsync(document);
    public Task DeleteOneAsync(string id)
    {
        FilterDefinition<TCollection> filter = CreateFilterById(id);
        return _collection.DeleteOneAsync(filter);
    }

    public Task<List<TCollection>> GetAllAsync(SortDefinition<TCollection>? sort = null)
    {
        FilterDefinition<TCollection> filter = Builders<TCollection>.Filter.Empty;
        IFindFluent<TCollection, TCollection> find = _collection.Find(filter);

        if (sort != null)
        {
            find.Sort(sort);
        }

        return find.ToListAsync();
    }

    public async Task<bool> ExistsByIdAsync(string id)
    {
        FilterDefinition<TCollection> filter = CreateFilterById(id);
        long count = await _collection.CountDocumentsAsync(filter);
        return count > 0;
    }
}
