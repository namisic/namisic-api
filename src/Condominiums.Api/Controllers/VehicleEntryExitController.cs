using Condominiums.Api.Auth.Attributes;
using Condominiums.Api.Constants;
using Condominiums.Api.Models.DTOs.VehiclesEntryExit;
using Condominiums.Api.Services;
using Condominiums.Api.Services.Base;
using Microsoft.AspNetCore.Mvc;

namespace Condominiums.Api.Controllers;

/// <summary>
/// Endpoints that allows to manage vehicle entry or exit.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class VehicleEntryExitController : ControllerBase
{
    private readonly IVehicleEntryExitService _vehicleEntryExitService;

    public VehicleEntryExitController(IVehicleEntryExitService vehicleEntryExitService)
    {
        _vehicleEntryExitService = vehicleEntryExitService;
    }

    /// <summary>
    /// Allows querying vehicle entry and exit records.
    /// </summary>
    [AuthorizeRole(RoleNames.Administrator, RoleNames.SecurityGuard)]
    [HttpGet]
    [ProducesResponseType(typeof(List<VehicleEntryExitDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(VehicleEntryExitFilters filters)
    {
        if (filters.CurrentUser) filters.CreatedBy = User.Identity!.Name!;
        ServiceResult<List<VehicleEntryExitDto>> result = await _vehicleEntryExitService.FilterAsync(filters);
        return this.ActionResultByServiceResult(result);
    }

    /// <summary>
    /// Allows to create a new vehicle entry or exit.
    /// </summary>
    [AuthorizeRole(RoleNames.SecurityGuard)]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post(CreateVehicleEntryExitDto createVehicleEntryExitDto)
    {
        string userName = User.Identity!.Name!;
        ServiceResult result = await _vehicleEntryExitService.CreateAsync(createVehicleEntryExitDto, userName);
        return this.ActionResultByServiceResult(result);
    }
}
