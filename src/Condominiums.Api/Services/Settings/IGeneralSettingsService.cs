using Condominiums.Api.Models.DTOs.Settings;

namespace Condominiums.Api.Services.Settings;

/// <summary>
/// Defines the operations to manage general settings.
/// </summary>
public interface IGeneralSettingsService
{
    /// <summary>
    /// Allows to get the general settings.
    /// </summary>
    /// <returns>General settings.</returns>
    Task<GeneralSettingsDto> GetAsync();

    /// <summary>
    /// Allows to update the general settings.
    /// </summary>
    /// <param name="updateGeneralSettingsDto">Information to update.</param>
    /// <returns></returns>
    Task UpdateAsync(UpdateGeneralSettingsDto updateGeneralSettingsDto);
}
