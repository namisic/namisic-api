namespace Condominiums.Api.Models.DTOs.Residents;

/// <summary>
/// Represents the information of a resident when it is queried.
/// </summary>
public class ResidentDto
{
    /// <summary>
    /// The resident's identifier.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The resident's name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The resident's document type.
    /// </summary>
    public string? DocumentType { get; set; }

    /// <summary>
    /// The resident's document number.
    /// </summary>
    public string? DocumentNumber { get; set; }

    /// <summary>
    /// The resident's e-mail.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// The resident's cellphone.
    /// </summary>
    public string? Cellphone { get; set; }

    /// <summary>
    /// The resident type. Could be "owner", "tenant" or "resident".
    /// </summary>
    public string? ResidentType { get; set; }

    /// <summary>
    /// House or apartment number where the resident lives.
    /// </summary>
    public string ApartmentNumber { get; set; } = string.Empty;
}
