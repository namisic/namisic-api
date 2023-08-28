using System.ComponentModel.DataAnnotations;

namespace Condominiums.Api.Models.DTOs.Residents;

public class GetResidentsQuery
{
    public string? Name { get; set; }

    [MatchesValues(
        Constants.DocumentType.CedulaCiudadania,
        Constants.DocumentType.CedulaExtranjeria,
        Constants.DocumentType.TarjetaIdentidad
    )]
    public string? DocumentType { get; set; }
    public string? DocumentNumber { get; set; }
    public string? Email { get; set; }
    public string? Cellphone { get; set; }

    [MatchesValues(
        Constants.ResidentType.Owner,
        Constants.ResidentType.Resident,
        Constants.ResidentType.Tenant
    )]
    public string? ResidentType { get; set; }
    public string? ApartmentNumber { get; set; }
}