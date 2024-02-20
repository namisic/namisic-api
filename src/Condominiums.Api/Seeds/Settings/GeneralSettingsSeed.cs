
using AutoMapper;
using Condominiums.Api.Models.DTOs.Settings;
using Condominiums.Api.Options;
using Condominiums.Api.Stores.Settings;
using Microsoft.Extensions.Options;

namespace Condominiums.Api.Seeds.Settings;

public class GeneralSettingsSeed : IGeneralSettingsSeed
{
    private readonly IGeneralSettingsStore _generalSettingsStore;
    private readonly ILogger<GeneralSettingsSeed> _logger;
    private readonly IMapper _mapper;
    private readonly GeneralSettingsOptions _generalSettingsOptions;

    public GeneralSettingsSeed(
        IGeneralSettingsStore generalSettingsStore,
        ILogger<GeneralSettingsSeed> logger,
        IMapper mapper,
        IOptions<GeneralSettingsOptions> generalSettingsOptions
    )
    {
        _generalSettingsOptions = generalSettingsOptions.Value;
        _generalSettingsStore = generalSettingsStore;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task SeedAsync()
    {
        GeneralSettingsDto? generalSettingsDb = await _generalSettingsStore.GetAsync();

        if (generalSettingsDb == null)
        {
            _logger.LogInformation("Initializing '{0}' settings", _generalSettingsStore.Name);
            GeneralSettingsDto generalSettings = _mapper.Map<GeneralSettingsDto>(_generalSettingsOptions);
            await _generalSettingsStore.InsertAsync(generalSettings);
        }
    }
}
