using Condominiums.Api.Models.Entities;
using Condominiums.Api.Stores.Base;
using MongoDB.Driver;

namespace Condominiums.Api.Stores;

/// <summary>
/// Defines the custom methods to perform the storage of vehicle entry or exit.
/// </summary>
public interface IVehicleEntryExitStore : IStore<VehicleEntryExit>
{

}

/// <summary>
/// Implements the custom methods to perform the storage of vehicle entry or exit.
/// </summary>
public class VehicleEntryExitStore : StoreBase<VehicleEntryExit>, IVehicleEntryExitStore
{
    public VehicleEntryExitStore(IMongoDatabase database) : base("vehicle_entry_exit", database)
    {
    }
}
