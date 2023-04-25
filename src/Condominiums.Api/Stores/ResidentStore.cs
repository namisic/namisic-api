using Condominiums.Api.Models.Entities;
using Condominiums.Api.Stores.Base;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Condominiums.Api.Stores;

public interface IResidentStore : IStore<Resident>
{

}

public class ResidentStore : IResidentStore
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<Resident> _residentsCollection;

    public ResidentStore(IMongoDatabase database)
    {
        _database = database;
        _residentsCollection = database.GetCollection<Resident>("residents");
    }

    public Task<Resident?> GetByIdAsync(ObjectId id)
    {
        FilterDefinition<Resident> filter = Builders<Resident>.Filter.Eq(r => r.Id, id);

        return _residentsCollection.Find(filter).FirstOrDefaultAsync();
    }

    public Task InsertOneAsync(Resident document) => _residentsCollection.InsertOneAsync(document);
}
