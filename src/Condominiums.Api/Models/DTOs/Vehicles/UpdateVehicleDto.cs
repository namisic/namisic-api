using System.ComponentModel.DataAnnotations;

namespace Condominiums.Api.Models.DTOs.Vehicles;

public class UpdateVehicleDto : CreateVehicleDto
{
    /// <summary>
    /// Indicates the initial license plate number of the vehicle.
    /// </summary>
    [Required]
    [MaxLength(8)]
    public string InitialPlateNumber { get; set; } = string.Empty;
}
