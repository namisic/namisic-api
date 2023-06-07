using Condominiums.Api.Models.DTOs.VehiclesEntryExit;
using Condominiums.Api.Models.Entities;
using Condominiums.Api.Stores.Base;
using MongoDB.Driver;

namespace Condominiums.Api.Stores;

/// <summary>
/// Defines the custom methods to perform the storage of vehicle entry or exit.
/// </summary>
public interface IVehicleEntryExitStore : IStore<VehicleEntryExit>
{
    /// <summary>
    /// Allows to query vehicle entry and exit records by using filters.
    /// </summary>
    /// <param name="filters">filters to be performed in the query.</param>
    /// <returns>Records of vehicle entries and exits matching the given filters.</returns>
    Task<List<VehicleEntryExit>> FilterAsync(VehicleEntryExitFilters filters);
}

/// <summary>
/// Implements the custom methods to perform the storage of vehicle entry or exit.
/// </summary>
public class VehicleEntryExitStore : StoreBase<VehicleEntryExit>, IVehicleEntryExitStore
{
    public VehicleEntryExitStore(IMongoDatabase database) : base("vehicle_entry_exit", database)
    {
    }

    public Task<List<VehicleEntryExit>> FilterAsync(VehicleEntryExitFilters filters)
    {
        FilterDefinitionBuilder<VehicleEntryExit> filterBuilder = Builders<VehicleEntryExit>.Filter;
        SortDefinition<VehicleEntryExit> sorting = Builders<VehicleEntryExit>.Sort.Descending(v => v.CreationDate);
        var filterConditions = new List<FilterDefinition<VehicleEntryExit>>();

        if (!string.IsNullOrEmpty(filters.CreatedBy))
        {
            string createdBy = filters.CreatedBy.Trim().ToLower();
            filterConditions.Add(filterBuilder.Where(v => v.CreatedBy.Trim().ToLower() == createdBy));
        }

        if (!string.IsNullOrEmpty(filters.PlateNumber))
        {
            string plateNumber = filters.PlateNumber.Trim().ToLower();
            filterConditions.Add(filterBuilder.Where(v => v.PlateNumber.Trim().ToLower() == plateNumber));
        }

        if (!string.IsNullOrEmpty(filters.Type))
        {
            string type = filters.Type.Trim().ToLower();
            filterConditions.Add(filterBuilder.Where(v => v.Type.Trim().ToLower() == type));
        }

        if (filters.BeginCreationDate != null)
        {
            filterConditions.Add(filterBuilder.Gte(v => v.CreationDate, filters.BeginCreationDate));
        }

        if (filters.EndCreationDate != null)
        {
            filterConditions.Add(filterBuilder.Lte(v => v.CreationDate, filters.EndCreationDate));
        }

        FilterDefinition<VehicleEntryExit> compoundFilters = filterBuilder.And(filterConditions);
        return Collection.Find(compoundFilters).Sort(sorting).ToListAsync();
    }
}
