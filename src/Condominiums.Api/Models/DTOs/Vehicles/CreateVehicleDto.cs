using System.ComponentModel.DataAnnotations;

namespace Condominiums.Api.Models.DTOs.Vehicles;

/// <summary>
/// Information needed to create a vehicle.
/// </summary>
public class CreateVehicleDto
{
    /// <summary>
    /// The resident's Id.
    /// </summary>
    [Required]
    public string ResidentId { get; set; } = string.Empty;

    // TODO: Create validation attribute to allow only possible values.
    /// <summary>
    /// Indicates the type of vehicle. Possible values are "car" or "motorcycle". Default value is "car".
    /// </summary>
    [Required]
    public string Type { get; set; } = "car";

    /// <summary>
    /// Indicates the license plate number of the vehicle.
    /// </summary>
    [Required]
    [MaxLength(8)]
    public string PlateNumber { get; set; } = string.Empty;
}
