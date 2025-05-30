/*
API de Nami SIC, la aplicación de código abierto que permite administrar Condominios fácilmente.
Copyright (C) 2025  Oscar David Díaz Fortaleché  lechediaz@gmail.com

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
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
