using System.ComponentModel.DataAnnotations;

namespace Condominiums.Api.Models.DTOs.Vehicles;

public class DeleteVehicleDto
{
    /// <summary>
    /// The resident's Id.
    /// </summary>
    [Required]
    public string ResidentId { get; set; } = string.Empty;

    /// <summary>
    /// Indicates the license plate number of the vehicle to delete.
    /// </summary>
    [Required]
    public string PlateNumber { get; set; } = string.Empty;
}
