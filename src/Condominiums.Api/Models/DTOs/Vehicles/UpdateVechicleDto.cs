using System.ComponentModel.DataAnnotations;

namespace Condominiums.Api.Models.DTOs.Vehicles;

public class UpdateVechicleDto : CreateVehicleDto
{
    /// <summary>
    /// Indicates the initial license plate number of the vehicle.
    /// </summary>
    [Required]
    public string InitialPlateNumber { get; set; } = string.Empty;
}
