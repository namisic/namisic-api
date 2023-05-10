using Condominiums.Api.Models.Entities;
using Condominiums.Api.Stores.Base;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Condominiums.Api.Stores;

/// <summary>
/// Defines the custom methods to perform the storage of residents.
/// </summary>
public interface IResidentStore : IStore<Resident>
{
    /// <summary>
    /// Allows to update a resident.
    /// </summary>
    /// <param name="resident">The resident information to update.</param>
    Task UpdateOneAsync(Resident resident);

    /// <summary>
    /// Allows adding a vehicle to a resident.
    /// </summary>
    /// <param name="id">Resident's id.</param>
    /// <param name="vehicle">Vehicle information.</param>
    Task AddVehicleAsync(string id, Vehicle vehicle);

    /// <summary>
    /// Allows updating a vehicle to a resident.
    /// </summary>
    /// <param name="id">Resident's id.</param>
    /// <param name="initialPlateNumber">Vehicles's initial plate number..</param>
    /// <param name="vehicle">Vehicle information.</param>
    Task UpdateVehicleAsync(string id, string initialPlateNumber, Vehicle vehicle);
}

/// <summary>
/// Implements the custom methods to perform the storage of residents.
/// </summary>
public class ResidentStore : StoreBase<Resident>, IResidentStore
{
    public ResidentStore(IMongoDatabase database) : base("residents", database)
    {

    }

    public async Task AddVehicleAsync(string id, Vehicle vehicle)
    {
        FilterDefinition<Resident> filter = CreateFilterById(id);
        UpdateDefinition<Resident> update = Builders<Resident>.Update
            .Push(r => r.Vehicles, vehicle);
        await Collection.UpdateOneAsync(filter, update);
    }

    public async Task UpdateOneAsync(Resident resident)
    {
        FilterDefinition<Resident> filter = Builders<Resident>.Filter.Eq(r => r.Id, resident.Id);
        UpdateDefinition<Resident> update = Builders<Resident>.Update
            .Set(r => r.Name, resident.Name)
            .Set(r => r.ApartmentNumber, resident.ApartmentNumber);
        await Collection.UpdateOneAsync(filter, update);
    }

    public async Task UpdateVehicleAsync(string id, string initialPlateNumber, Vehicle vehicle)
    {
        ObjectId residentId = ObjectId.Parse(id);
        FilterDefinitionBuilder<Resident> filterBuilder = Builders<Resident>.Filter;
        FilterDefinition<Resident> filter = filterBuilder.And(
            filterBuilder.Eq(r => r.Id, residentId),
            filterBuilder.ElemMatch(r => r.Vehicles, v => v.PlateNumber == initialPlateNumber)
        );
        UpdateDefinition<Resident> update = Builders<Resident>.Update
            .Set(r => r.Vehicles.FirstMatchingElement(), vehicle);
        await Collection.UpdateOneAsync(filter, update);
    }
}
