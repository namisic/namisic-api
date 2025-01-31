using Condominiums.Api.Seeds.Base;
using Condominiums.Api.Seeds.Settings;

namespace Condominiums.Api.Seeds;

public class Farmer : IFarmer
{
    private readonly ILogger<Farmer> _logger;
    private readonly IGeneralSettingsSeed _generalSettingsSeed;

    public Farmer(ILogger<Farmer> logger, IGeneralSettingsSeed generalSettingsSeed)
    {
        _logger = logger;
        _generalSettingsSeed = generalSettingsSeed;
    }

    public async Task PlantAsync()
    {
        _logger.LogTrace("Planting seeds");

        try
        {
            await _generalSettingsSeed.SeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error plantando semillas: {error}", [new { error = ex.Message }]);
        }
    }
}
