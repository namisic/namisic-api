using Condominiums.Api.Models.DTOs.Settings;
using Condominiums.Api.Stores.Settings.Base;
using MongoDB.Driver;

namespace Condominiums.Api.Stores.Settings;

public class GeneralSettingsStore : BaseSettingsStore<GeneralSettingsDto>, IGeneralSettingsStore
{
    public GeneralSettingsStore(IMongoDatabase database) : base(Constants.Database.Settings.General, database)
    {
    }
}
