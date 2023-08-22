using System.ComponentModel.DataAnnotations;

namespace Condominiums.Api.Models.DTOs.Residents;

/// <summary>
/// Represents the information needed to update a resident.
/// </summary>
public class UpdateResidentDto
{
    /// <summary>
    /// The resident's name.
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The resident's document type.
    /// </summary>
    [MatchesValues(
        Constants.DocumentType.CedulaCiudadania,
        Constants.DocumentType.CedulaExtranjeria,
        Constants.DocumentType.TarjetaIdentidad
    )]
    public string DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// The resident's document number.
    /// </summary>
    public string DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// The resident's e-mail.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The resident's cellphone.
    /// </summary>
    public string Cellphone { get; set; } = string.Empty;

    /// <summary>
    /// The resident type. Could be "owner", "tenant" or "resident".
    /// </summary>
    [MatchesValues(
        Constants.ResidentType.Owner,
        Constants.ResidentType.Resident,
        Constants.ResidentType.Tenant
    )]
    public string ResidentType { get; set; } = string.Empty;

    /// <summary>
    /// House or apartment number where the resident lives.
    /// </summary>
    [Required]
    public string ApartmentNumber { get; set; } = string.Empty;
}
