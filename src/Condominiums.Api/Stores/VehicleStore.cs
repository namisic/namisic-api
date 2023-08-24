using Condominiums.Api.Models.Entities;
using Condominiums.Api.Stores.Base;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Condominiums.Api.Stores;

/// <summary>
/// Defines the custom methods to perform the storage of vehicles.
/// </summary>
public interface IVehicleStore : IStore<Resident>
{
    /// <summary>
    /// Allows to find a resident by searching for a vehicle with a license plate number.
    /// </summary>
    /// <param name="plateNumber">The license plate number to search.</param>
    /// <param name="ignoreId">Resident ID to ignore.</param>
    /// <returns>The resident found, otherwise <see langword="null"/></returns>
    Task<Resident?> FindResidentByVehiclePlateNumberAsync(string plateNumber, string? ignoreId = null);

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

    /// <summary>
    /// Allows to filter the plate numbers of vehicles given a portion of this.
    /// </summary>
    /// <param name="plateNumberHint">Plate number portion.</param>
    /// <returns>List of plate numbers.</returns>
    Task<List<string>> FilterPlateNumbersAsync(string plateNumberHint);

    /// <summary>
    /// Allows to validate if a vehicle exists by searching for its license plate number.
    /// </summary>
    /// <param name="plateNumber">The license plate number to search.</param>
    /// <returns><see langword="true"/> if vehicle exists otherwise <see langword="false"/>.</returns>
    Task<bool> ValidateIfVehicleExistsAsync(string plateNumber);

    /// <summary>
    /// Allows to search a vehicle for its license plate number.
    /// </summary>
    /// <param name="plateNumber">The license plate number to search.</param>
    /// <returns>The vehicle record.</returns>
    Task<Vehicle?> GetVehicleByPlateNumberAsync(string plateNumber);
}

/// <summary>
/// Implements the custom methods to perform the storage of vehicles.
/// </summary>
public class VehicleStore : StoreBase<Resident>, IVehicleStore
{
    public VehicleStore(IMongoDatabase database) : base("residents", database)
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

    public async Task<List<string>> FilterPlateNumbersAsync(string plateNumberHint)
    {
        plateNumberHint = plateNumberHint.ToLower().Trim();
        List<string> plateNumbers = await Collection.AsQueryable()
            .SelectMany(r => r.Vehicles.Select(v => v.PlateNumber))
            .Where(p => p.ToLower().Trim().StartsWith(plateNumberHint))
            .ToListAsync();
        return plateNumbers;
    }

    public async Task<Resident?> FindResidentByVehiclePlateNumberAsync(string plateNumber, string? ignoreId = null)
    {
        plateNumber = plateNumber.ToLower().Trim();
        FilterDefinitionBuilder<Resident> filterBuilder = Builders<Resident>.Filter;
        FilterDefinition<Resident> baseFilter = Builders<Resident>.Filter.ElemMatch(
            r => r.Vehicles,
            v => v.PlateNumber.ToLower() == plateNumber
        );
        FilterDefinition<Resident> filter = baseFilter;
        if (!string.IsNullOrEmpty(ignoreId))
        {
            ObjectId objectId = ObjectId.Parse(ignoreId);
            filter = filterBuilder.And(baseFilter, filterBuilder.Ne(r => r.Id, objectId));
        }
        Resident? resident = await Collection.Find(filter).FirstOrDefaultAsync();
        return resident;
    }

    public async Task<Vehicle?> GetVehicleByPlateNumberAsync(string plateNumber)
    {
        plateNumber = plateNumber.Trim().ToLower();
        Vehicle? vehicle = await Collection.AsQueryable()
            .SelectMany(r => r.Vehicles)
            .FirstOrDefaultAsync(v => v.PlateNumber.Trim().ToLower() == plateNumber);
        return vehicle;
    }

    public async Task<List<Vehicle>> GetVehiclesAsync(string id)
    {
        FilterDefinition<Resident> filter = CreateFilterById(id);
        Resident? resident = await Collection.Find(filter).FirstOrDefaultAsync();
        return resident?.Vehicles ?? new List<Vehicle>();
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

    public async Task<bool> ValidateIfVehicleExistsAsync(string plateNumber)
    {
        plateNumber = plateNumber.Trim().ToLower();
        FilterDefinition<Resident> filter = Builders<Resident>.Filter.ElemMatch(
            r => r.Vehicles,
            v => v.PlateNumber.Trim().ToLower() == plateNumber
        );
        long count = await Collection.CountDocumentsAsync(filter);
        return count > 0;
    }
}