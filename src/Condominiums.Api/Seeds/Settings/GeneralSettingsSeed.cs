
using Condominiums.Api.Models.DTOs.Settings;
using Condominiums.Api.Stores.Settings;
using Microsoft.Extensions.Options;

namespace Condominiums.Api.Seeds.Settings;

public class GeneralSettingsSeed : IGeneralSettingsSeed
{
    private readonly GeneralSettings _generalSettings;
    private readonly ILogger<GeneralSettingsSeed> _logger;
    private readonly IGeneralSettingsStore _generalSettingsStore;

    public GeneralSettingsSeed(
        ILogger<GeneralSettingsSeed> logger,
        IOptions<GeneralSettings> generalSettingsOptions,
        IGeneralSettingsStore generalSettingsStore
    )
    {
        _generalSettings = generalSettingsOptions.Value;
        _logger = logger;
        _generalSettingsStore = generalSettingsStore;
    }

    public async Task SeedAsync()
    {
        GeneralSettings? generalSettingsDb = await _generalSettingsStore.GetAsync();

        if (generalSettingsDb == null)
        {
            _logger.LogInformation("Initializing '{0}' settings", _generalSettingsStore.Name);
            await _generalSettingsStore.InsertAsync(_generalSettings);
        }
    }
}
