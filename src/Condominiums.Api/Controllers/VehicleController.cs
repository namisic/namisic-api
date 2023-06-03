using Condominiums.Api.Auth.Attributes;
using Condominiums.Api.Models.DTOs.Vehicles;
using Condominiums.Api.Services;
using Condominiums.Api.Services.Base;
using Microsoft.AspNetCore.Mvc;

namespace Condominiums.Api.Controllers;

/// <summary>
/// Endpoints that allows to manage Vehicles.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly IResidentService _residentService;

    public VehiclesController(IResidentService residentService)
    {
        _residentService = residentService;
    }

    /// <summary>
    /// Allows you to obtain all the vehicles that belong to a resident by ID.
    /// </summary>
    [ProducesResponseType(typeof(List<VehicleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}")]
    [AuthorizeRole(Constants.RoleNames.Administrator)]
    public async Task<IActionResult> Get(string id)
    {
        ServiceResult<List<VehicleDto>> result = await _residentService.GetVehiclesAsync(id);
        return this.ActionResultByServiceResult(result);
    }

    /// <summary>
    /// Allows to filter the plate numbers of vehicles given a portion of this.
    /// </summary>
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("filter-plate-numbers")]
    [AuthorizeRole(Constants.RoleNames.SecurityGuard)]
    public async Task<IActionResult> FilterPlateNumbers(string plateNumberHint)
    {
        ServiceResult<List<string>> result = await _residentService.FilterPlateNumbersAsync(plateNumberHint);
        return this.ActionResultByServiceResult(result);
    }

    /// <summary>
    /// Allows to create a new vehicle.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    [AuthorizeRole(Constants.RoleNames.Administrator)]
    public async Task<IActionResult> Post(CreateVehicleDto newVehicle)
    {
        ServiceResult result = await _residentService.AddVehicleAsync(newVehicle);
        return this.ActionResultByServiceResult(result);
    }

    /// <summary>
    /// Allows to update a vehicle.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut]
    [AuthorizeRole(Constants.RoleNames.Administrator)]
    public async Task<IActionResult> Put(UpdateVehicleDto vehicle)
    {
        ServiceResult result = await _residentService.UpdateVehicleAsync(vehicle);
        return this.ActionResultByServiceResult(result);
    }

    /// <summary>
    /// Allows to delete a vehicle.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete]
    [AuthorizeRole(Constants.RoleNames.Administrator)]
    public async Task<IActionResult> Delete(DeleteVehicleDto vehicle)
    {
        ServiceResult result = await _residentService.DeleteVehicleAsync(vehicle);
        return this.ActionResultByServiceResult(result);
    }
}
