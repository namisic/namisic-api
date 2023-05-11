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
    /// Allows to obtain all the vehicles that belong to a resident by ID.
    /// </summary>
    /// <param name="id">Resident's id.</param>
    Task<List<Vehicle>> GetVehiclesAsync(string id);

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
    /// <param name="initialPlateNumber">Vehicles's initial plate number.</param>
    /// <param name="vehicle">Vehicle information.</param>
    Task UpdateVehicleAsync(string id, string initialPlateNumber, Vehicle vehicle);

    /// <summary>
    /// Allows updating a vehicle to a resident.
    /// </summary>
    /// <param name="id">Resident's id.</param>
    /// <param name="plateNumber">Vehicles's initial plate number.</param>
    Task DeleteVehicleAsync(string id, string plateNumber);
}

/// <summary>
/// Implements the custom methods to perform the storage of residents.
/// </summary>
public class ResidentStore : StoreBase<Resident>, IResidentStore
{
    public ResidentStore(IMongoDatabase database) : base("residents", database)
    {

    }

    public Task AddVehicleAsync(string id, Vehicle vehicle)
    {
        FilterDefinition<Resident> filter = CreateFilterById(id);
        UpdateDefinition<Resident> update = Builders<Resident>.Update
            .Push(r => r.Vehicles, vehicle);
        return Collection.UpdateOneAsync(filter, update);
    }

    public Task DeleteVehicleAsync(string id, string plateNumber)
    {
        ObjectId residentId = ObjectId.Parse(id);
        FilterDefinitionBuilder<Resident> filterBuilder = Builders<Resident>.Filter;
        plateNumber = plateNumber.ToUpperInvariant();
        FilterDefinition<Resident> filter = filterBuilder.And(
            filterBuilder.Eq(r => r.Id, residentId),
            filterBuilder.ElemMatch(r => r.Vehicles, v => v.PlateNumber == plateNumber)
        );
        UpdateDefinition<Resident> update = Builders<Resident>.Update
            .PullFilter(r => r.Vehicles, v => v.PlateNumber == plateNumber);
        return Collection.UpdateOneAsync(filter, update);

    }

    public async Task<List<Vehicle>> GetVehiclesAsync(string id)
    {
        FilterDefinition<Resident> filter = CreateFilterById(id);
        Resident? resident = await Collection.Find(filter).FirstOrDefaultAsync();
        return resident?.Vehicles ?? new List<Vehicle>();
    }

    public Task UpdateOneAsync(Resident resident)
    {
        FilterDefinition<Resident> filter = Builders<Resident>.Filter.Eq(r => r.Id, resident.Id);
        UpdateDefinition<Resident> update = Builders<Resident>.Update
            .Set(r => r.Name, resident.Name)
            .Set(r => r.ApartmentNumber, resident.ApartmentNumber);
        return Collection.UpdateOneAsync(filter, update);
    }

    public Task UpdateVehicleAsync(string id, string initialPlateNumber, Vehicle vehicle)
    {
        ObjectId residentId = ObjectId.Parse(id);
        FilterDefinitionBuilder<Resident> filterBuilder = Builders<Resident>.Filter;
        initialPlateNumber = initialPlateNumber.ToUpperInvariant();
        FilterDefinition<Resident> filter = filterBuilder.And(
            filterBuilder.Eq(r => r.Id, residentId),
            filterBuilder.ElemMatch(r => r.Vehicles, v => v.PlateNumber == initialPlateNumber)
        );
        UpdateDefinition<Resident> update = Builders<Resident>.Update
            .Set(r => r.Vehicles.FirstMatchingElement(), vehicle);
        return Collection.UpdateOneAsync(filter, update);
    }
}
