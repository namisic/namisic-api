using System.ComponentModel.DataAnnotations;

namespace Condominiums.Api.Models.DTOs.Residents;

/// <summary>
/// Represents the information needed to update a resident.
/// </summary>
public class UpdateResidentDto
{
    private string? _documentType;
    private string? _residentType;

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
    public string? DocumentType { get => _documentType; set => _documentType = value?.ToLower(); }

    /// <summary>
    /// The resident's document number.
    /// </summary>
    public string? DocumentNumber { get; set; }

    /// <summary>
    /// The resident's e-mail.
    /// </summary>
    [EmailAddress]
    public string? Email { get; set; }

    /// <summary>
    /// The resident's cellphone.
    /// </summary>
    [Phone]
    public string? Cellphone { get; set; }

    /// <summary>
    /// The resident type. Could be "owner", "tenant" or "resident".
    /// </summary>
    [MatchesValues(
        Constants.ResidentType.Owner,
        Constants.ResidentType.Resident,
        Constants.ResidentType.Tenant
    )]
    public string? ResidentType { get => _residentType; set => _residentType = value?.ToLower(); }

    /// <summary>
    /// House or apartment number where the resident lives.
    /// </summary>
    [Required]
    public string ApartmentNumber { get; set; } = string.Empty;
}
