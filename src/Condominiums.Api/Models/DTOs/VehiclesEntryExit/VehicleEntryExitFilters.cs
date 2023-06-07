using System.ComponentModel.DataAnnotations;
using Condominiums.Api.Constants;

namespace Condominiums.Api.Models.DTOs.VehiclesEntryExit;

/// <summary>
/// Filters that can be used when querying vehicle entry and exit records.
/// </summary>
public class VehicleEntryExitFilters
{
    public string? PlateNumber { get; set; }

    [MatchesValues(VehicleEntryExitType.Entry, VehicleEntryExitType.Exit)]
    public string? Type { get; set; }
    public DateTime? BeginCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public string? CreatedBy { get; set; }
    public bool CurrentUser { get; set; }
}
