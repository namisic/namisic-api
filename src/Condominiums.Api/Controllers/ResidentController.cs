using Condominiums.Api.Models.DTOs.Residents;
using Condominiums.Api.Services;
using Condominiums.Api.Services.Base;
using Microsoft.AspNetCore.Mvc;

namespace Condominiums.Api.Controllers;

/// <summary>
/// Endpoints that allows to manage Residents.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ResidentController : ControllerBase
{
    private readonly IResidentService _residentService;

    public ResidentController(IResidentService residentService)
    {
        _residentService = residentService;
    }

    /// <summary>
    /// Allows to create a new Resident.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> Post(CreateResidentDto newResident)
    {
        ServiceResult result = await _residentService.CreateAsync(newResident);
        return this.ActionResultByServiceResult(result);
    }
}
