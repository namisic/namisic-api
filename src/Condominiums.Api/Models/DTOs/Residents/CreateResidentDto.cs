using System.ComponentModel.DataAnnotations;

namespace Condominiums.Api.Models.DTOs.Residents;

/// <summary>
/// Information neeeded to create a resident.
/// </summary>
public class CreateResidentDto
{
    /// <summary>
    /// The resident's name.
    /// </summary>
    [Required]
    public string Name { get; set; } = String.Empty;

    /// <summary>
    /// House or apartment number where the resident lives.
    /// </summary>
    [Required]
    public string ApartmentNumber { get; set; } = String.Empty;
}
