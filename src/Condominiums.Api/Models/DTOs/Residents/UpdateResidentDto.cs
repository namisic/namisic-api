namespace Condominiums.Api.Models.DTOs.Residents;

/// <summary>
/// Represents the information needed to update a resident.
/// </summary>
public class UpdateResidentDto
{
    /// <summary>
    /// The resident's name.
    /// </summary>
    public string Name { get; set; } = String.Empty;

    /// <summary>
    /// House or apartment number where the resident lives.
    /// </summary>
    public string ApartmentNumber { get; set; } = String.Empty;
}
