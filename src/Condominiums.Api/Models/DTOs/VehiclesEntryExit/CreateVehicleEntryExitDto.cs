using System.ComponentModel.DataAnnotations;

namespace Condominiums.Api.Models.DTOs.VehiclesEntryExit;

/// <summary>
/// Represents the information necessary to record a vehicle entry or exit.
/// </summary>
public class CreateVehicleEntryExitDto
{
    /// <summary>
    /// Indicates the license plate number of the vehicle.
    /// </summary>
    [Required]
    [MaxLength(8)]
    public string PlateNumber { get; set; } = string.Empty;

    /// <summary>
    /// Indicates the type of vehicle. Possible values are "entry" or "exit".
    /// </summary>
    // TODO: Create validation attribute to allow only possible values.
    [Required]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// indicates any observation about the event.
    /// </summary>
    public string? Remarks { get; set; }
}
