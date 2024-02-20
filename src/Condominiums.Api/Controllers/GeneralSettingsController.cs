using Condominiums.Api.Auth.Attributes;
using Condominiums.Api.Models.DTOs.Settings;
using Condominiums.Api.Services.Settings;
using Microsoft.AspNetCore.Mvc;

namespace Condominiums.Api.Controllers;

/// <summary>
/// General settings operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GeneralSettingsController : ControllerBase
{
    private readonly IGeneralSettingsService _generalSettingsService;

    public GeneralSettingsController(IGeneralSettingsService generalSettingsService)
    {
        _generalSettingsService = generalSettingsService;
    }

    [HttpGet]
    public async Task<ActionResult<GeneralSettingsDto>> Get()
    {
        GeneralSettingsDto generalSettingsDto = await _generalSettingsService.GetAsync();
        return Ok(generalSettingsDto);
    }

    [AuthorizeRole(Constants.RoleNames.Administrator)]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateGeneralSettingsDto updateGeneralSettingsDto)
    {
        await _generalSettingsService.UpdateAsync(updateGeneralSettingsDto);
        return Ok();
    }
}
