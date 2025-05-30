/*
API de Nami SIC, la aplicación de código abierto que permite administrar Condominios fácilmente.
Copyright (C) 2025  Oscar David Díaz Fortaleché  lechediaz@gmail.com

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
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

        if (!string.IsNullOrEmpty(filters.VehicleType))
        {
            string vehicleType = filters.VehicleType.Trim().ToLower();
            filterConditions.Add(filterBuilder.Where(v => v.VehicleType != null && v.VehicleType.Trim().ToLower() == vehicleType));
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
