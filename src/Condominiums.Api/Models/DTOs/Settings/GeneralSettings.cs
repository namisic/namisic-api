namespace Condominiums.Api.Models.DTOs.Settings;

public class GeneralSettings
{
    public static readonly string SettingName = "general";
    public string CondominiumName { get; set; }
    public string Description { get; set; }
    public string BackgroundImagePath { get; set; }
    public string Location { get; set; }
    public string Phone { get; set; }
    public string CoexistenceManualPath { get; set; }
}
