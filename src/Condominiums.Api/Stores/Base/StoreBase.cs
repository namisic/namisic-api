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

    public Task<TCollection?> GetByIdAsync(string id)
    {
        ObjectId objectId = ObjectId.Parse(id);
        FilterDefinition<TCollection> filter = Builders<TCollection>.Filter.Eq(r => r.Id, objectId);
        return _collection.Find(filter).FirstOrDefaultAsync();
    }

    public Task InsertOneAsync(TCollection document) => _collection.InsertOneAsync(document);
    public Task DeleteOneAsync(string id)
    {
        ObjectId objectId = ObjectId.Parse(id);
        FilterDefinition<TCollection> filter = Builders<TCollection>.Filter.Eq(r => r.Id, objectId);
        return _collection.DeleteOneAsync(filter);
    }
}
