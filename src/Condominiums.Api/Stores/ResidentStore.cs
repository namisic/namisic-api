using Condominiums.Api.Models.Entities;
using Condominiums.Api.Stores.Base;
using MongoDB.Driver;

namespace Condominiums.Api.Stores;

public interface IResidentStore : IStore<Resident>
{

}

public class ResidentStore : StoreBase<Resident>, IResidentStore
{
    public ResidentStore(IMongoDatabase database) : base("residents", database)
    {

    }
}
