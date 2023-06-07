namespace Condominiums.Api.Models.DTOs.VehiclesEntryExit;

/// <summary>
/// Information to be displayed when querying vehicle entry/exit records.
/// </summary>
public class VehicleEntryExitDto
{
    public string Id { get; set; } = string.Empty;
    public string PlateNumber { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? Remarks { get; set; }
    public DateTime CreationDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}
