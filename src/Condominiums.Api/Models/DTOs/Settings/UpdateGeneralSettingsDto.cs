namespace Condominiums.Api.Models.DTOs.Settings;

public class UpdateGeneralSettingsDto
{
    public string CondominiumName { get; set; }
    public string CondominiumDescription { get; set; }
    public string CondominiumAddress { get; set; }
    public string CondominiumPhone { get; set; }
}
