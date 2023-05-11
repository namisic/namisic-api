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
    /// Allows to create a new vehicle.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
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
    public async Task<IActionResult> Delete(DeleteVehicleDto vehicle)
    {
        ServiceResult result = await _residentService.DeleteVehicleAsync(vehicle);
        return this.ActionResultByServiceResult(result);
    }
}
