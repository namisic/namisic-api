namespace Condominiums.Api.Models.DTOs.Vehicles;

/// <summary>
/// Represents the information of a vehicle when it is queried.
/// </summary>
public class VehicleDto
{
    /// <summary>
    /// Indicates the type of vehicle. Possible values are "car" or "motorcycle". Default value is "car".
    /// </summary>
    public string Type { get; set; } = "car";

    /// <summary>
    /// Indicates the license plate number of the vehicle.
    /// </summary>
    public string PlateNumber { get; set; } = string.Empty;
}
