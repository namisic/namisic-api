using Condominiums.Api.Models.DTOs.Settings;
using Condominiums.Api.Stores.Settings;
using Microsoft.AspNetCore.Mvc;

namespace Condominiums.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IGeneralSettingsStore _generalSettingsStore;

    public TestController(IGeneralSettingsStore generalSettingsStore)
    {
        _generalSettingsStore = generalSettingsStore;
    }

    [HttpGet]
    public async Task<GeneralSettings?> Get()
    {
        GeneralSettings? settings = await _generalSettingsStore.GetAsync();
        return settings;
    }
}
