namespace Condominiums.Api.Models.DTOs.Settings;

public class GeneralSettings
{
    public static readonly string SettingName = "general";
    public string CondominiumName { get; set; }
    public string CondominiumDescription { get; set; }
    public string CondominiumLocation { get; set; }
    public string CondominiumPhone { get; set; }
    public string CondominiumCoexistenceManualPath { get; set; }
    public string HomePageBackgroundImagePath { get; set; }
}
