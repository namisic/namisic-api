using Condominiums.Api.Auth.Attributes;
using Condominiums.Api.Constants;
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
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    /// <summary>
    /// Allows you to obtain all the vehicles that belong to a resident by ID.
    /// </summary>
    [ProducesResponseType(typeof(List<VehicleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}")]
    [AuthorizeRole(RoleNames.Administrator)]
    public async Task<IActionResult> Get(string id)
    {
        ServiceResult<List<VehicleDto>> result = await _vehicleService.GetVehiclesAsync(id);
        return this.ActionResultByServiceResult(result);
    }

    /// <summary>
    /// Allows you to obtain a vehicle by its license plate number.
    /// </summary>
    [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("get-by-plate-number")]
    [AuthorizeRole(RoleNames.Administrator)]
    public async Task<IActionResult> GetByPlateNumber(string plateNumber)
    {
        ServiceResult<VehicleDto> result = await _vehicleService.GetVehicleByPlateNumberAsync(plateNumber);
        return this.ActionResultByServiceResult(result);
    }

    /// <summary>
    /// Allows to filter the plate numbers of vehicles given a portion of this.
    /// </summary>
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("filter-plate-numbers")]
    [AuthorizeRole(RoleNames.SecurityGuard)]
    public async Task<IActionResult> FilterPlateNumbers(string plateNumberHint)
    {
        ServiceResult<List<string>> result = await _vehicleService.FilterPlateNumbersAsync(plateNumberHint);
        return this.ActionResultByServiceResult(result);
    }

    /// <summary>
    /// Allows to create a new vehicle.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    [AuthorizeRole(RoleNames.Administrator)]
    public async Task<IActionResult> Post(CreateVehicleDto newVehicle)
    {
        ServiceResult result = await _vehicleService.AddVehicleAsync(newVehicle);
        return this.ActionResultByServiceResult(result);
    }

    /// <summary>
    /// Allows to update a vehicle.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut]
    [AuthorizeRole(RoleNames.Administrator)]
    public async Task<IActionResult> Put(UpdateVehicleDto vehicle)
    {
        ServiceResult result = await _vehicleService.UpdateVehicleAsync(vehicle);
        return this.ActionResultByServiceResult(result);
    }

    /// <summary>
    /// Allows to delete a vehicle.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete]
    [AuthorizeRole(RoleNames.Administrator)]
    public async Task<IActionResult> Delete(DeleteVehicleDto vehicle)
    {
        ServiceResult result = await _vehicleService.DeleteVehicleAsync(vehicle);
        return this.ActionResultByServiceResult(result);
    }
}
