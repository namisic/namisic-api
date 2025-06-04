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
using Condominiums.Api.Base;

namespace Condominiums.Api.Services.Settings;

public interface IGeneralSettingsImageService
{
    /// <summary>
    /// Uploads the home background image and updates the general settings with the public URI.
    /// </summary>
    /// <param name="fileStream">The image file stream.</param>
    /// <param name="extension">The file extension (e.g., ".jpg").</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Result with the public URI of the uploaded image.</returns>
    Task<Result<string>> UploadHomeBackgroundImageAsync(Stream fileStream, string extension, CancellationToken cancellationToken = default);
}
