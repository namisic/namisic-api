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
    private readonly IGeneralSettingsImageService _generalSettingsImageService;
    private readonly IGeneralSettingsCoexistenceManualService _generalSettingsCoexistenceManualService;

    public GeneralSettingsController(
        IGeneralSettingsService generalSettingsService,
        IGeneralSettingsImageService generalSettingsImageService,
        IGeneralSettingsCoexistenceManualService generalSettingsCoexistenceManualService)
    {
        _generalSettingsService = generalSettingsService;
        _generalSettingsImageService = generalSettingsImageService;
        _generalSettingsCoexistenceManualService = generalSettingsCoexistenceManualService;
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

    [AuthorizeRole(Constants.RoleNames.Administrator)]
    [HttpPut("home-background-image")]
    public async Task<IActionResult> UploadHomeBackgroundImage([FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file provided.");

        var extension = Path.GetExtension(file.FileName);
        using var fileStream = file.OpenReadStream();

        var result = await _generalSettingsImageService.UploadHomeBackgroundImageAsync(
            fileStream, extension, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(new { url = result.Value });
    }

    [AuthorizeRole(Constants.RoleNames.Administrator)]
    [HttpPut("coexistence-manual")]
    public async Task<IActionResult> UploadCoexistenceManual([FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file provided.");

        var extension = Path.GetExtension(file.FileName);
        using var fileStream = file.OpenReadStream();

        var result = await _generalSettingsCoexistenceManualService.UploadCoexistenceManualAsync(
            fileStream, extension, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(new { url = result.Value });
    }
}
