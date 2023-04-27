namespace Condominiums.Api.Models.DTOs.Residents;

/// <summary>
/// Represents the information of a resident when is queried.
/// </summary>
public class ResidentDto
{
    /// <summary>
    /// The resident's identifier.
    /// </summary>
    public string Id { get; set; } = String.Empty;

    /// <summary>
    /// The resident's name.
    /// </summary>
    public string Name { get; set; } = String.Empty;

    /// <summary>
    /// House or apartment number where the resident lives.
    /// </summary>
    public string ApartmentNumber { get; set; } = String.Empty;
}
